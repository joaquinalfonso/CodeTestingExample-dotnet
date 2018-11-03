using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebApi.Models;
using WebApi.Comun;
using WebApi.App_Code;

namespace WebApi.Controllers
{
    public class TestController : ApiController
    {

        private ITranscripcionesBO bo;

        public TestController()
        {
            this.bo = new TranscripcionesBO();
        }

        public TestController(ITranscripcionesBO transcripcionesBO)
        {
            this.bo = transcripcionesBO;
        }


        public HttpResponseMessage Get(string desde = "", string hasta = "")
        {

            try
            {
                ParametrosGetTranscripcionesTO parametrosConsulta = new TranscripcionesBR().ObtenerParametrosConsultaGetTranscripciones(Request, desde, hasta);
                 
                parametrosConsulta.Login = new TranscripcionesBR().ObtenerUsuarioDeRequestYValidarAcceso(Request);

                List<TranscripcionDTO> listaTranscripciones = bo.ObtenerTranscripciones(parametrosConsulta);

                return Request.CreateResponse(HttpStatusCode.OK, listaTranscripciones);

            }
            catch (HttpResponseException ex)
            {
                switch (ex.Response.StatusCode)
                {
                    case HttpStatusCode.Unauthorized:
                        return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Usuario no válido");

                    case HttpStatusCode.BadRequest:
                        string mensajeError = string.Format("Formato de fechas incorrecto. Parámetros opcionales: ?desde={0}&hasta={0}", Configuracion.FORMATO_FECHA_VARIABLE_QUERYSTRING);
                        return Request.CreateErrorResponse(HttpStatusCode.BadRequest, mensajeError);
                  
                    default:
                        return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Ha ocurrido un error");

                }

            }



            //var queryString = Request.GetQueryNameValuePairs();

            //DateTime fechaDesde = DateTime.MinValue;
            //DateTime fechaHasta = DateTime.MaxValue;

            //string formatoFecha = "yyyy-MM-ddTHH:mm";
            //try
            //{
            //    if (desde != "")
            //        fechaDesde = DateTime.ParseExact(desde, formatoFecha, System.Globalization.CultureInfo.InvariantCulture);
            //    if (hasta != "")
            //        fechaHasta = DateTime.ParseExact(hasta, formatoFecha, System.Globalization.CultureInfo.InvariantCulture);

            //}
            //catch (Exception ex)
            //{
            //    return Request.CreateResponse(HttpStatusCode.BadRequest, "Formato de fechas incorrecto");
            //}

            //string mensaje = "Metodo Get all ";
            //if (fechaDesde != DateTime.MinValue)
            //    mensaje += " Desde " + fechaDesde.ToString();

            //if (fechaHasta != DateTime.MaxValue)
            //    mensaje += " Hasta " + fechaHasta.ToString();


            
        }



        // api/Transcripciones/{id}
        public HttpResponseMessage Get(int id)
        {
            try
            {
                string loginUsuario = new TranscripcionesBR().ObtenerUsuarioDeRequestYValidarAcceso(Request);
                Transcripcion transcripcion = new TranscripcionesBR().ObtenerTranscripcionRealizada(bo, id, loginUsuario);
                string textoTranscrito = new TranscripcionesBR().ObtenerFicheroTranscritoTxt(transcripcion);

                return Request.CreateResponse(HttpStatusCode.OK, textoTranscrito);
            }
            catch (HttpResponseException ex)
            {
                switch (ex.Response.StatusCode)
                {
                    case HttpStatusCode.Unauthorized:
                        return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Usuario no válido");

                    case HttpStatusCode.NotFound:
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Transcripción no encontrada");

                    case HttpStatusCode.NoContent:
                        return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "La transcripción aún no se ha realizado");

                    case HttpStatusCode.InternalServerError:
                        return Request.CreateErrorResponse(HttpStatusCode.InternalServerError, "Ha ocurrido un error al realizar la transcripción");

                    default:
                        return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Ha ocurrido un error");

                }

            }

        }
    }
}
