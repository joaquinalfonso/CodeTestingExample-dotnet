using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;
using WebApi.App_Code;
using WebApi.Comun;
using WebApi.Models;

namespace WebApi.Controllers
{
    public class TranscripcionesController : ApiController
    {
        
        private ITranscripcionesBO bo;

        public TranscripcionesController()
        {
            this.bo = new TranscripcionesBO();
        }

        public TranscripcionesController(ITranscripcionesBO transcripcionesBO)
        {
            this.bo = transcripcionesBO;
        }


        //[System.Web.Http.Route("Transcripciones/{desde:DateTime?}&{hasta:DateTime?}")]
        //[System.Web.Http.AcceptVerbs("GET")]
        //[System.Web.Http.HttpGet]
        //public IHttpActionResult GetAllTranscripciones(DateTime? desde = null, DateTime? hasta = null)
        //{
        //    ParametrosGetTranscripcionesTO parametrosRequest = new ParametrosGetTranscripcionesTO();
        //    parametrosRequest.Desde = desde;
        //    parametrosRequest.Hasta = hasta;

        //    List<TranscripcionDTO> listaTranscripciones = bo.ObtenerTranscripciones(parametrosRequest);


        //    //TODO: Si la lista es vacia devolver un 204            
        //    return Ok(listaTranscripciones);
        //}

        public IHttpActionResult GetAllTranscripciones()
        {
            string loginUsuario = new TranscripcionesBR().ObtenerUsuarioDeRequestYValidarAcceso(Request);

            ParametrosGetTranscripcionesTO parametrosRequest = new ParametrosGetTranscripcionesTO { Login = loginUsuario };

            //TODO: Validar formato de fechas
            if (parametrosRequest.Login == null)
            {
                return BadRequest();
            }

            List<TranscripcionDTO> listaTranscripciones = bo.ObtenerTranscripciones(parametrosRequest);


            //TODO: Si la lista es vacia devolver un 204            
            return Ok(listaTranscripciones);

        }


        public IHttpActionResult GetAllTranscripcionesFecha([FromUri]ParametrosGetTranscripcionesTO parametrosRequest)
        {
            string loginUsuario = new TranscripcionesBR().ObtenerUsuarioDeRequestYValidarAcceso(Request);

            parametrosRequest.Login = loginUsuario;
            //ParametrosGetTranscripcionesTO parametrosRequest = new ParametrosGetTranscripcionesTO { Login = loginUsuario, Desde = desde, Hasta = hasta };

            //TODO: Validar formato de fechas
            if (parametrosRequest.Login == null)
            {
                return BadRequest();
            }

            List<TranscripcionDTO> listaTranscripciones = bo.ObtenerTranscripciones(parametrosRequest);


            //TODO: Si la lista es vacia devolver un 204            
            return Ok(listaTranscripciones);

        }

        /*
    [ResponseType(typeof(TranscripcionDTO))]
    public IHttpActionResult GetAllTranscripciones([FromUri]ParametrosGetTranscripcionesTO parametrosRequest)
    {
        //string loginUsuario = new ApiBR().ObtenerUsuarioDeRequestYValidarAcceso(Request);

        //TODO: Validar formato de fechas
        if (parametrosRequest.Login == null)
        {
            return BadRequest();
        }

        List<TranscripcionDTO> listaTranscripciones = bo.ObtenerTranscripciones(parametrosRequest);


        //TODO: Si la lista es vacia devolver un 204            
        return Ok(listaTranscripciones);

    }
    */

        // GET: api/Transcripcion/5
        //[ResponseType(typeof(Transcripcion))]
        public IHttpActionResult GetTranscripcion(int id)
        {             
            string loginUsuario = new TranscripcionesBR().ObtenerUsuarioDeRequestYValidarAcceso(Request);

            Transcripcion transcripcion = bo.ObtenerTranscripcion(id, loginUsuario);
                
            // Flujo Alternativo A
            if (transcripcion == null)
            {
                return NotFound();
            }

            // Flujo Alternativo B
            if (transcripcion.Estado == TipoEstadoTranscripcion.PENDIENTE.ToString() || 
                transcripcion.Estado == TipoEstadoTranscripcion.EN_PROGRESO.ToString())
            {
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.BadRequest, "La transcripción aún no se ha realizado"));
            }

            // Flujo Alternativo C
            if (transcripcion.Estado == TipoEstadoTranscripcion.ERROR.ToString())
            {
                return ResponseMessage(Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Ha ocurrido un error al realizar la transcripción"));
            }
            

            if (transcripcion.Estado == TipoEstadoTranscripcion.REALIZADA.ToString() && 
                new FicherosBR().ExisteFicheroTranscritoTxt(transcripcion.Id))
            {
                return Ok(new FicherosBR().ObtenerFicheroTranscritoTxt(transcripcion.Id));
            }

            return NotFound();
        }

        



        // PUT: api/Transcripciones/5
        //[ResponseType(typeof(void))]
        //public IHttpActionResult PutTranscripciones(string id, Transcripciones transcripciones)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    if (id != transcripciones.Id)
        //    {
        //        return BadRequest();
        //    }

        //    db.Entry(transcripciones).State = EntityState.Modified;

        //    try
        //    {
        //        db.SaveChanges();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!TranscripcionesExists(id))
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return StatusCode(HttpStatusCode.NoContent);
        //}

        //// POST: api/Transcripciones
        //[ResponseType(typeof(Transcripciones))]
        //public IHttpActionResult PostTranscripciones(Transcripciones transcripciones)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return BadRequest(ModelState);
        //    }

        //    db.Transcripciones.Add(transcripciones);

        //    try
        //    {
        //        db.SaveChanges();
        //    }
        //    catch (DbUpdateException)
        //    {
        //        if (TranscripcionesExists(transcripciones.Id))
        //        {
        //            return Conflict();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return CreatedAtRoute("DefaultApi", new { id = transcripciones.Id }, transcripciones);
        //}

        public HttpResponseMessage PostTranscripciones(string Login)
        {
            //string loginUsuario = new ApiBR().ObtenerUsuarioDeRequestYValidarAcceso(Request);

            // Validar login 
            // Validar tamaño y tipo
            // Pasar a clase BO login y archivo

            
            try
            {
                var httpRequest = HttpContext.Current.Request;
                if (httpRequest.Files.Count < 1)
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest);
                }

                foreach (string file in httpRequest.Files)
                {
                    var postedFile = httpRequest.Files[file];

                    try
                    {
                        new TranscripcionesBR().ValidarFichero(postedFile);
                    }
                    catch (HttpResponseException excepcion)
                    {
                        string mensajeError = "";
                        mensajeError += excepcion.Response.StatusCode == HttpStatusCode.RequestEntityTooLarge ? "Archivo excede tamaño máximo (5Mb)" : "";
                        mensajeError += excepcion.Response.StatusCode == HttpStatusCode.UnsupportedMediaType ? "Formato de archivo no válido" : "";

                        return Request.CreateErrorResponse(HttpStatusCode.BadRequest, mensajeError);
                    }

                    int idTranscripcion = bo.ObtenerNuevoIdTranscripcion();

                    new TranscripcionesBR().GrabarFicheroMp3(postedFile, idTranscripcion);

                    Transcripcion transcripcion = new Transcripcion
                    {
                        Id = idTranscripcion,
                        FechaHoraRecepcion = DateTime.Now,
                        LoginUsuario = Login,
                        NombreArchivo = postedFile.FileName,
                        Estado = TipoEstadoTranscripcion.PENDIENTE.ToString()
                    };

                    bo.InsertarTranscripcion(transcripcion);

                }

            }
            catch(Exception excepcion)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, excepcion.Message);
            }
            

            return Request.CreateResponse(HttpStatusCode.Created, "La llamada se ha procesado con éxito");
        }

        

        // DELETE: api/Transcripciones/5
        //[ResponseType(typeof(Transcripciones))]
        //public IHttpActionResult DeleteTranscripciones(string id)
        //{
        //    Transcripciones transcripciones = db.Transcripciones.Find(id);
        //    if (transcripciones == null)
        //    {
        //        return NotFound();
        //    }

        //    db.Transcripciones.Remove(transcripciones);
        //    db.SaveChanges();

        //    return Ok(transcripciones);
        //}

        //protected override void Dispose(bool disposing)
        //{
        //    if (disposing)
        //    {
        //        db.Dispose();
        //    }
        //    base.Dispose(disposing);
        //}

        //private bool TranscripcionesExists(string id)
        //{
        //    return db.Transcripciones.Count(e => e.Id == id) > 0;
        //}
    }
}