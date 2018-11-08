using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using WebApi.Comun;
using WebApi.Infraestructura;
using WebApi.INVOX;
using WebApi.Models;

namespace WebApi.Servicios
{

    // Clase de servicios que implementa la logica de negocio de las operaciones de transcripciones.
    // Implementa la interface ITranscripcionesService.

    public class TranscripcionesService : ITranscripcionesService
    {
        //public VocaliEntities db { get; set; }

        public IFicherosResource ficherosResource { private get; set; }
        public IBaseDatosResource baseDatosResource { private get; set; }
        public IConfiguracionResource configuracionResource { private get; set; }

        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        public TranscripcionesService()
        {
            //db = new VocaliEntities();
            ficherosResource = new FicherosResource();
            baseDatosResource = new BaseDatosResource();
            configuracionResource = new ConfiguracionResource();
        }

        private Transcription ObtenerTranscripcionRealizada(int id, string login)
        {
            Transcription transcripcion = baseDatosResource.ObtenerTranscripcion(id, login);

            // Flujo Alternativo A
            if (transcripcion == null)
            {
                throw new TranscripcionNoEncontradaException();
            }

            // Flujo Alternativo B
            if (transcripcion.Estado == (int)TipoEstadoTranscripcion.PENDIENTE ||
                transcripcion.Estado == (int)TipoEstadoTranscripcion.EN_PROGRESO)
            {
                throw new TranscripcionPendienteException();
            }

            // Flujo Alternativo C
            if (transcripcion.Estado == (int)TipoEstadoTranscripcion.ERROR)
            {
                throw new TranscripcionErroneaException();
            }

            return transcripcion;
        }

        private string ObtenerNombreFicheroTranscritoTxt(int id)
        {
            return string.Format("{0}.txt", id);
         }

        private string ObtenerFicheroTranscritoTxt(Transcription transcripcion)
        {

            string texto = "";

            if (transcripcion == null)
            {
                throw new TranscripcionNoEncontradaException();
            }

            if (transcripcion.Estado != (int)TipoEstadoTranscripcion.REALIZADA)
            {
                throw new TranscripcionPendienteException();
            }

            try
            {
                string nombreFicheroTranscrito = ObtenerNombreFicheroTranscritoTxt(transcripcion.Id);

                if (ficherosResource.ExisteFicheroTranscritoTxt(nombreFicheroTranscrito))
                {
                    texto = ficherosResource.ObtenerFicheroTranscritoTxt(nombreFicheroTranscrito);
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex, ex.Message);
                throw new TranscripcionErroneaException("ObtenerFicheroTranscritoTxt", ex);
            }

            if (String.IsNullOrEmpty(texto))
            {
                throw new TranscripcionErroneaException();
            }

            return texto;

        }

        public void ProcesarTranscripciones(List<Transcription> transcripcionesPendientes)
        {
            var cacheTranscripcionesPendientes = new CacheTranscripcionesPendientes(transcripcionesPendientes);
            var procesos = new List<Task>();

            for (int contador = 0; contador < configuracionResource.ObtenerConfiguracion().NUMERO_HILOS_PROCESAMIENTO_TRANSCRIPCIONES; contador++)
            {
                procesos.Add(Task.Run(() =>
                {
                    while (cacheTranscripcionesPendientes.HayTranscripcionesPendientes)
                    {
                        Transcription transcripcionPendiente = cacheTranscripcionesPendientes.ObtenerSiguienteTranscripcionPendiente();

                        if (transcripcionPendiente != null)
                        {
                            logger.Trace("Proceso {0} Transcripcion {1}", Task.CurrentId, transcripcionPendiente.Id);

                            ProcesarTranscripcion(transcripcionPendiente);
                        }
                    }
                }));
            }


        }


        private string ObtenerNombreFicheroMp3(Transcription transcripcion)
        {
            return string.Format("{0}{1}", transcripcion.Id, configuracionResource.ObtenerConfiguracion().EXTENSION_FICHEROS_AUDIO.ToLower());
        }

        public void ProcesarTranscripcion(Transcription transcripcion)
        {
            TipoEstadoTranscripcion nuevoEstadoTranscripcion = TipoEstadoTranscripcion.EN_PROGRESO;
            string mensajeError = "";

            try
            {

                string nombreFichero = ObtenerNombreFicheroMp3(transcripcion);

                Byte[] ficheroMp3 = ficherosResource.ObtenerFicheroMp3(nombreFichero);

                string textoTranscrito = new INVOXMedicalMock().TranscribirFicheroMp3(transcripcion.LoginUsuario, ficheroMp3);

                string nombreFicheroTranscrito = ObtenerNombreFicheroTranscritoTxt(transcripcion.Id);

                ficherosResource.GrabarFicheroTextoTranscrito(nombreFicheroTranscrito, textoTranscrito);

                nuevoEstadoTranscripcion = TipoEstadoTranscripcion.REALIZADA;

            }
            catch (Exception ex)
            {
                nuevoEstadoTranscripcion = TipoEstadoTranscripcion.ERROR;
                mensajeError = ex.Message;

                logger.Error(ex, ex.Message);
            }
            finally
            {
                transcripcion.Estado = (int)nuevoEstadoTranscripcion;
                transcripcion.FechaHoraTranscripcion = DateTime.Now;
                transcripcion.MensajeError = mensajeError;
                baseDatosResource.ActualizarTranscripcion(transcripcion);
            }
        }

        public void ProcesarTranscripcionesPendientes()
        {
            List<Transcription> transcripcionesPendientes = baseDatosResource.ObtenerTranscripcionesPendientes();

            logger.Info("{0} transcripciones pendientes de procesar", transcripcionesPendientes.Count);


            if (transcripcionesPendientes.Count > 0)
                ProcesarTranscripciones(transcripcionesPendientes);
        }

        public void RecibirFicheroATranscribir(HttpPostedFile fichero, string login)
        {
            try
            {
                
                Transcription transcripcion = new Transcription
                {
                    FechaHoraRecepcion = DateTime.Now,
                    LoginUsuario = login,
                    NombreArchivo = fichero.FileName,
                    Estado = (int)TipoEstadoTranscripcion.PENDIENTE
                };

                int nuevoIdTranscripcion = baseDatosResource.InsertarTranscripcion(transcripcion);

                string nombreFichero = ObtenerNombreFicheroMp3(transcripcion);

                ficherosResource.GrabarFicheroMp3(fichero, nombreFichero);
            }
            catch (Exception ex)
            {
                logger.Error(ex, ex.Message);
                throw new TranscripcionNoGuardadaException("RecibirFicheroATranscribir", ex);
            }
        }

        public string ObtenerTextoTranscripcionRealizada(int id, string login)
        {
            Transcription transcripcion = ObtenerTranscripcionRealizada(id, login);
            string textoTranscrito = ObtenerFicheroTranscritoTxt(transcripcion);
            return textoTranscrito;
        }

        public List<TranscripcionDTO> ObtenerTranscripciones(ParametrosConsultaTranscripcionesTO parametros)
        {
            return baseDatosResource.ObtenerTranscripciones(parametros);
        }
    }
}