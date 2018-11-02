using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using WebApiCrud.App_Code;
using WebApiCrud.Comun;
using WebApiCrud.INVOX;
using WebApiCrud.Models;

namespace WebApiCrud.Controllers
{
    public class TranscripcionesBO : ITranscripcionesBO
    {
        private VocaliEntities db = new VocaliEntities();
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        public Transcripciones ObtenerTranscripcion(string id, string login)
        {
            return db.Transcripciones.FirstOrDefault((p) => p.Id == id && p.LoginUsuario == login);
        }

        public List<TranscripcionDTO> ObtenerTranscripciones(ParametrosGetTranscripcionesTO parametros)
        {

            IQueryable<Transcripciones> list = from g in db.Transcripciones
                                               where g.LoginUsuario == parametros.Login &&
                                                    (g.FechaHoraRecepcion >= parametros.Desde || parametros.Desde == null) &&
                                                    (g.FechaHoraRecepcion <= parametros.Hasta || parametros.Hasta == null)
                                               select g;

            List<TranscripcionDTO> listaTranscripciones = new List<TranscripcionDTO>();
            list.ToList().ForEach(x => listaTranscripciones.Add(new TranscripcionDTO(x)));

            return listaTranscripciones;

        }

        public void ActualizarTranscripcion(Transcripciones transcripcion)
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

        public void ProcesarTranscripciones(List<Transcripciones> transcripcionesPendientes)
        {
            
            //Random r = new Random();
            var cacheTranscripcionesPendientes = new CacheTranscripcionesPendientes(transcripcionesPendientes);
            var procesos = new List<Task>();

            for (int contador = 0; contador < Configuracion.NUMERO_HILOS_PROCESAMIENTO_TRANSCRIPCIONES; contador++)
            {
                procesos.Add(Task.Run(() =>
                {
                    while (cacheTranscripcionesPendientes.HayTranscripcionesPendientes)
                    {
                        Transcripciones transcripcionPendiente = cacheTranscripcionesPendientes.ObtenerSiguienteTranscripcionPendiente();

                        if (transcripcionPendiente != null)
                        {
                            logger.Trace("Proceso {0} Transcripcion {1}", Task.CurrentId, transcripcionPendiente.Id);

                            ProcesarTranscripcion(transcripcionPendiente);
                        }
                    }
                }));
            }


        }

        public void ProcesarTranscripcion(Transcripciones transcripcion)
        {
            string nuevoEstadoTranscripcion = TipoEstadoTranscripcion.EN_PROGRESO.ToString();
            string mensajeError = "";

            try
            {
               
                string textoTranscrito = new INVOXMedicalMock().TranscribirFicheroMp3(transcripcion.LoginUsuario, null);
                
                GrabarFicheroTextoTranscrito(transcripcion, textoTranscrito);

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

        private void GrabarFicheroTextoTranscrito(Transcripciones transcripcion, string textoTranscrito)
        {
            string rutaFicheroTranscrito = ObtenerRutaFicheroTranscritoTxt(transcripcion);

            File.WriteAllText(rutaFicheroTranscrito, textoTranscrito);
        }

        private string ObtenerRutaFicheroTranscritoTxt(Transcripciones transcripcion)
        {
            string rutaFicherosTranscripciones = System.Web.Hosting.HostingEnvironment.MapPath(Configuracion.RUTA_FICHEROS_TRANSCRITOS);
            var rutaFicheroTxt = rutaFicherosTranscripciones + string.Format("{0}.txt", transcripcion.Id);
            return rutaFicheroTxt;
        }

        public bool ExisteFicheroTranscritoTxt(Transcripciones transcripcion)
        {
            string rutaFicheroTxt = ObtenerRutaFicheroTranscritoTxt(transcripcion);

            return File.Exists(rutaFicheroTxt);
        }

        public string ObtenerFicheroTranscritoTxt(Transcripciones transcripcion)
        {
            string rutaFicheroTxt = ObtenerRutaFicheroTranscritoTxt(transcripcion);

            string contenidoFicheroTxt = "";
            using (StreamReader sr = new StreamReader(rutaFicheroTxt, System.Text.Encoding.Default, false))
            {
                contenidoFicheroTxt = sr.ReadToEnd().Replace("\r\n", " ");
            }

            return contenidoFicheroTxt;
        }

        public void ProcesarTranscripcionesPendientes()
        {
            List<Transcripciones> transcripcionesPendientes = ObtenerTranscripcionesPendientes();

            logger.Info("{0} transcripciones pendientes de procesar", transcripcionesPendientes.Count);


            if (transcripcionesPendientes.Count > 0)
                ProcesarTranscripciones(transcripcionesPendientes);
        }

        private List<Transcripciones> ObtenerTranscripcionesPendientes()
        {
            IQueryable<Transcripciones> list = from g in db.Transcripciones
                                               where g.Estado == TipoEstadoTranscripcion.PENDIENTE.ToString()
                                               select g;

            return (list.ToList());
        }

        public void InsertarTranscripcion(Transcripciones transcripcion)
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
    }
}