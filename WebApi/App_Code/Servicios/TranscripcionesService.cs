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

        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        public TranscripcionesService()
        {
            //db = new VocaliEntities();
            ficherosResource = new FicherosResource();
            baseDatosResource = new BaseDatosResource();
        }

        private Transcripcion ObtenerTranscripcionRealizada(int id, string login)
        {
            Transcripcion transcripcion = baseDatosResource.ObtenerTranscripcion(id, login);

            // Flujo Alternativo A
            if (transcripcion == null)
            {
                throw new TranscripcionNoEncontradaException();
            }

            // Flujo Alternativo B
            if (transcripcion.Estado == TipoEstadoTranscripcion.PENDIENTE.ToString() ||
                transcripcion.Estado == TipoEstadoTranscripcion.EN_PROGRESO.ToString())
            {
                throw new TranscripcionPendienteException();
            }

            // Flujo Alternativo C
            if (transcripcion.Estado == TipoEstadoTranscripcion.ERROR.ToString())
            {
                throw new TranscripcionErroneaException();
            }

            return transcripcion;
        }

        private string ObtenerFicheroTranscritoTxt(Transcripcion transcripcion)
        {

            string texto = "";

            if (transcripcion == null)
            {
                throw new TranscripcionNoEncontradaException();
            }

            if (transcripcion.Estado != TipoEstadoTranscripcion.REALIZADA.ToString())
            {
                throw new TranscripcionPendienteException();
            }

            try
            {
                if (ficherosResource.ExisteFicheroTranscritoTxt(transcripcion.Id))
                {
                    texto = ficherosResource.ObtenerFicheroTranscritoTxt(transcripcion.Id);
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

        public void ProcesarTranscripciones(List<Transcripcion> transcripcionesPendientes)
        {
            var cacheTranscripcionesPendientes = new CacheTranscripcionesPendientes(transcripcionesPendientes);
            var procesos = new List<Task>();

            for (int contador = 0; contador < Configuracion.NUMERO_HILOS_PROCESAMIENTO_TRANSCRIPCIONES; contador++)
            {
                procesos.Add(Task.Run(() =>
                {
                    while (cacheTranscripcionesPendientes.HayTranscripcionesPendientes)
                    {
                        Transcripcion transcripcionPendiente = cacheTranscripcionesPendientes.ObtenerSiguienteTranscripcionPendiente();

                        if (transcripcionPendiente != null)
                        {
                            logger.Trace("Proceso {0} Transcripcion {1}", Task.CurrentId, transcripcionPendiente.Id);

                            ProcesarTranscripcion(transcripcionPendiente);
                        }
                    }
                }));
            }


        }

        public void ProcesarTranscripcion(Transcripcion transcripcion)
        {
            string nuevoEstadoTranscripcion = TipoEstadoTranscripcion.EN_PROGRESO.ToString();
            string mensajeError = "";

            try
            {
                Byte[] ficheroMp3 = ficherosResource.ObtenerFicheroMp3(transcripcion.Id);

                string textoTranscrito = new INVOXMedicalMock().TranscribirFicheroMp3(transcripcion.LoginUsuario, ficheroMp3);

                ficherosResource.GrabarFicheroTextoTranscrito(transcripcion.Id, textoTranscrito);

                nuevoEstadoTranscripcion = TipoEstadoTranscripcion.REALIZADA.ToString();

            }
            catch (Exception ex)
            {
                nuevoEstadoTranscripcion = TipoEstadoTranscripcion.ERROR.ToString();
                mensajeError = ex.Message;

                logger.Error(ex, ex.Message);
            }
            finally
            {
                transcripcion.Estado = nuevoEstadoTranscripcion;
                transcripcion.FechaHoraTranscripcion = DateTime.Now;
                transcripcion.MensajeError = mensajeError;
                baseDatosResource.ActualizarTranscripcion(transcripcion);
            }
        }

        public void ProcesarTranscripcionesPendientes()
        {
            List<Transcripcion> transcripcionesPendientes = baseDatosResource.ObtenerTranscripcionesPendientes();

            logger.Info("{0} transcripciones pendientes de procesar", transcripcionesPendientes.Count);


            if (transcripcionesPendientes.Count > 0)
                ProcesarTranscripciones(transcripcionesPendientes);
        }

        public void RecibirFicheroATranscribir(HttpPostedFile fichero, string login)
        {
            try
            {
                int nuevoIdTranscripcion = baseDatosResource.ObtenerNuevoIdTranscripcion();

                ficherosResource.GrabarFicheroMp3(fichero, nuevoIdTranscripcion);

                Transcripcion transcripcion = new Transcripcion
                {
                    Id = nuevoIdTranscripcion,
                    FechaHoraRecepcion = DateTime.Now,
                    LoginUsuario = login,
                    NombreArchivo = fichero.FileName,
                    Estado = TipoEstadoTranscripcion.PENDIENTE.ToString()
                };

                baseDatosResource.InsertarTranscripcion(transcripcion);
            }
            catch (Exception ex)
            {
                logger.Error(ex, ex.Message);
                throw new TranscripcionNoGuardadaException("RecibirFicheroATranscribir", ex);
            }
        }

        public string ObtenerTextoTranscripcionRealizada(int id, string login)
        {
            Transcripcion transcripcion = ObtenerTranscripcionRealizada(id, login);
            string textoTranscrito = ObtenerFicheroTranscritoTxt(transcripcion);
            return textoTranscrito;
        }

        public List<TranscripcionDTO> ObtenerTranscripciones(ParametrosConsultaTranscripcionesTO parametros)
        {
            return baseDatosResource.ObtenerTranscripciones(parametros);
        }
    }
}