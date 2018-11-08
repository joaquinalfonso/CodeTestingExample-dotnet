using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using WebApi.Comun;
using WebApi.Models;


namespace WebApi.Infraestructura
{
    public class BaseDatosResource : IBaseDatosResource
    {

        public WebApiDBContext db { get; set; }
      
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        public BaseDatosResource()
        {
            db = new WebApiDBContext();            
        }

        public List<Transcription> ObtenerTranscripcionesPendientes()
        {
            IQueryable<Transcription> list = from g in db.Transcriptions
                                             where g.Estado == (int)TipoEstadoTranscripcion.PENDIENTE
                                             select g;

            return (list.ToList());
        }

        public List<TranscripcionDTO> ObtenerTranscripciones(ParametrosConsultaTranscripcionesTO parametros)
        {

            IQueryable<Transcription> list = from g in db.Transcriptions
                                             where g.LoginUsuario == parametros.Login &&
                                                  (g.FechaHoraRecepcion >= parametros.FechaDesde || parametros.FechaDesde == null) &&
                                                  (g.FechaHoraRecepcion <= parametros.FechaHasta || parametros.FechaHasta == null)
                                             select g;

            List<TranscripcionDTO> listaTranscripciones = new List<TranscripcionDTO>();
            list.ToList().ForEach(x => listaTranscripciones.Add(new TranscripcionDTO(x)));

            return listaTranscripciones;

        }

       

        public Transcription ObtenerTranscripcion(int id, string login)
        {
            return db.Transcriptions.FirstOrDefault((p) => p.Id == id && p.LoginUsuario == login);
        }

        private WebApiDBContext ObtenerContextoDB()
        {
            return new WebApiDBContext();
        }

        public void ActualizarTranscripcion(Transcription transcripcion)
        {
            //Se crea un dbcontext nuevo porque no soporta operaciones con hilos
            WebApiDBContext dbIndependiente = ObtenerContextoDB();

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

        public int InsertarTranscripcion(Transcription transcripcion)
        {
            //VocaliEntities dbIndependiente = ObtenerContextoDB();
            int nuevoId = 0;

            db.Transcriptions.Add(transcripcion);

            try
            {
                db.SaveChanges();
                nuevoId = transcripcion.Id;
                return nuevoId;
            }
            catch (Exception ex)
            {
                logger.Error(ex);
                throw;
            }
        }
    }
}