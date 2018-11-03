using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using WebApi.App_Code;
using WebApi.Comun;
using WebApi.INVOX;
using WebApi.Models;

namespace WebApi.Controllers
{
    public class TranscripcionesBO : ITranscripcionesBO
    {
        private VocaliEntities db = new VocaliEntities();
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        public Transcripcion ObtenerTranscripcion(int id, string login)
        {
            return db.Transcripciones.FirstOrDefault((p) => p.Id == id && p.LoginUsuario == login);
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

        public void ActualizarTranscripcion(Transcripcion transcripcion)
        {
            //Se crea un dbcontext nuevo porque no soporta operaciones con hilos
            VocaliEntities dbIndependiente = new VocaliEntities();

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
                Byte[] ficheroMp3 = new FicherosBR().ObtenerFicheroMp3(transcripcion.Id);
                
                string textoTranscrito = new INVOXMedicalMock().TranscribirFicheroMp3(transcripcion.LoginUsuario, ficheroMp3);

                new FicherosBR().GrabarFicheroTextoTranscrito(transcripcion.Id, textoTranscrito);

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
            VocaliEntities dbIndependiente = new VocaliEntities();

            dbIndependiente.Transcripciones.Add(transcripcion);

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

        public int ObtenerNuevoIdTranscripcion()
        {
            int? maxId = db.Transcripciones.Max(x => (int?)x.Id);
            int nuevoId = (maxId != null) ? (int)maxId + 1 : 1;

            return nuevoId;
        }

        
    }
}