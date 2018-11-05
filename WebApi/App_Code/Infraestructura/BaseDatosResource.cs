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

        public VocaliEntities db { get; set; }
      
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        public BaseDatosResource()
        {
            db = new VocaliEntities();            
        }

        public List<Transcripcion> ObtenerTranscripcionesPendientes()
        {
            IQueryable<Transcripcion> list = from g in db.Transcripciones
                                             where g.Estado == TipoEstadoTranscripcion.PENDIENTE.ToString()
                                             select g;

            return (list.ToList());
        }

        public List<TranscripcionDTO> ObtenerTranscripciones(ParametrosConsultaTranscripcionesTO parametros)
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

        public int ObtenerNuevoIdTranscripcion()
        {
            //TODO: Implementar como secuencia en EF
            int? maxId = db.Transcripciones.Max(x => (int?)x.Id);
            int nuevoId = (maxId != null) ? (int)maxId + 1 : 1;

            return nuevoId;
        }

        public Transcripcion ObtenerTranscripcion(int id, string login)
        {
            return db.Transcripciones.FirstOrDefault((p) => p.Id == id && p.LoginUsuario == login);
        }

        private VocaliEntities ObtenerContextoDB()
        {
            return new VocaliEntities();
        }

        public void ActualizarTranscripcion(Transcripcion transcripcion)
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
    }
}