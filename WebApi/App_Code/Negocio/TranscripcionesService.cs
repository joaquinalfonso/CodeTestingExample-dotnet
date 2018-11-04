using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using WebApi.Comun;
using WebApi.INVOX;
using WebApi.Models;
using WebApi.Infraestructura;

namespace WebApi.Negocio
{
    public class TranscripcionesService : ITranscripcionesService
    {
        public VocaliEntities db { get; set; }
        public VocaliEntities dbPruebas { private get; set; }
        public IFicherosResource ficherosResource { private get; set; }

        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        

        public TranscripcionesService()
        {
            db = new VocaliEntities();
            ficherosResource = new FicherosResource();
        }

        
       


        private Transcripcion ObtenerTranscripcionRealizada(int id, string login)
        {
            Transcripcion transcripcion = db.Transcripciones.FirstOrDefault((p) => p.Id == id && p.LoginUsuario == login);

            // Flujo Alternativo A
            if (transcripcion == null)
            {
                throw new TranscripcionNoEncontradaException();
                //  HttpResponseException(HttpStatusCode.NotFound);
            }

            // Flujo Alternativo B
            if (transcripcion.Estado == TipoEstadoTranscripcion.PENDIENTE.ToString() ||
                transcripcion.Estado == TipoEstadoTranscripcion.EN_PROGRESO.ToString())
            {
                throw new TranscripcionNoEncontradaException();
                //throw new HttpResponseException(HttpStatusCode.NoContent);
            }

            // Flujo Alternativo C
            if (transcripcion.Estado == TipoEstadoTranscripcion.ERROR.ToString())
            {
                throw new TranscripcionNoEncontradaException();
                //throw new HttpResponseException(HttpStatusCode.InternalServerError);
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


       

        

        public List<TranscripcionDTO> ObtenerTranscripciones(ParametrosGetTranscripcionesTO parametros)
        {

            IQueryable<Transcripcion> list = from g in db.Transcripciones
                                             where g.LoginUsuario == parametros.Login &&
                                                  (g.FechaHoraRecepcion >= parametros.FechaDesde || parametros.FechaDesde == null) &&
                                                  (g.FechaHoraRecepcion <= parametros.FechaHasta || parametros.FechaHasta == null)
                                             select g;

            List<TranscripcionDTO> listaTranscripciones = new List<TranscripcionDTO>();
            list.ToList().ForEach(x => listaTranscripciones.Add(new TranscripcionDTO(x)));

            return listaTranscripciones;

        }

        private VocaliEntities ObtenerContextoDB()
        {
            return (dbPruebas == null) ? new VocaliEntities() : dbPruebas;
        }

        private void ActualizarTranscripcion(Transcripcion transcripcion)
        {
            //Se crea un dbcontext nuevo porque no soporta operaciones con hilos
            VocaliEntities dbIndependiente = ObtenerContextoDB();

            dbIndependiente.Entry(transcripcion).State = EntityState.Modified;

            try
            {
                dbIndependiente.SaveChanges();
            }
            catch (Exception ex)
            {
                logger.Error(ex);
                throw;
            }

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
                ActualizarTranscripcion(transcripcion);
            }
        }

        public void ProcesarTranscripcionesPendientes()
        {
            List<Transcripcion> transcripcionesPendientes = ObtenerTranscripcionesPendientes();

            logger.Info("{0} transcripciones pendientes de procesar", transcripcionesPendientes.Count);


            if (transcripcionesPendientes.Count > 0)
                ProcesarTranscripciones(transcripcionesPendientes);
        }

        private List<Transcripcion> ObtenerTranscripcionesPendientes()
        {
            IQueryable<Transcripcion> list = from g in db.Transcripciones
                                             where g.Estado == TipoEstadoTranscripcion.PENDIENTE.ToString()
                                             select g;

            return (list.ToList());
        }

        public void InsertarTranscripcion(Transcripcion transcripcion)
        {
            //VocaliEntities dbIndependiente = ObtenerContextoDB();

            db.Transcripciones.Add(transcripcion);
            
            try
            {
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                logger.Error(ex);
                throw;
            }
        }

        public int ObtenerNuevoIdTranscripcion()
        {
            //TODO: Implementar como secuencia en EF
            int? maxId = db.Transcripciones.Max(x => (int?)x.Id);
            int nuevoId = (maxId != null) ? (int)maxId + 1 : 1;

            return nuevoId;
        }

        public void RecibirFicheroATranscribir(HttpPostedFile fichero, string login)
        {
            try
            {
                int nuevoIdTranscripcion = ObtenerNuevoIdTranscripcion();

                ficherosResource.GrabarFicheroMp3(fichero, nuevoIdTranscripcion);

                Transcripcion transcripcion = new Transcripcion
                {
                    Id = nuevoIdTranscripcion,
                    FechaHoraRecepcion = DateTime.Now,
                    LoginUsuario = login,
                    NombreArchivo = fichero.FileName,
                    Estado = TipoEstadoTranscripcion.PENDIENTE.ToString()
                };

                InsertarTranscripcion(transcripcion);
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
    }
}