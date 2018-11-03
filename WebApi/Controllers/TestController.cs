using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebApi.Models;
using WebApi.Comun;
using WebApi.App_Code;
using System.Web;

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

        // [GET] api/Transcripciones
        // [GET] api/Transcripciones?desde=yyyy-MM-ddTHH:mm
        // [GET] api/Transcripciones?hasta=yyyy-MM-ddTHH:mm
        // [GET] api/Transcripciones?desde=yyyy-MM-ddTHH:mm&hasta=yyyy-MM-ddTHH:mm
        public HttpResponseMessage GetTranscripciones(string desde = "", string hasta = "")
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
                return GestionarErrorGetTranscripciones(ex);
            }

        }

        private HttpResponseMessage GestionarErrorGetTranscripciones(HttpResponseException ex)
        {
            switch (ex.Response.StatusCode)
            {
                case HttpStatusCode.Unauthorized:
                    return Request.CreateErrorResponse(HttpStatusCode.Unauthorized, Configuracion.ObtenerMensajeTexto("UsuarioNoValido"));

                case HttpStatusCode.BadRequest:
                    string mensajeError = string.Format(Configuracion.ObtenerMensajeTexto("FormatoIncorrectoGetTranscripciones"), Configuracion.FORMATO_FECHA_VARIABLE_QUERYSTRING);
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, mensajeError);

                default:
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, Configuracion.ObtenerMensajeTexto("HaOcurridoUnError"));
            }
        }

        // [GET] api/Transcripciones/{id}
        public HttpResponseMessage GetTranscripcion(int id)
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
                return GestionarErrorGetTranscripcion(ex);
            }

        }

        private HttpResponseMessage GestionarErrorGetTranscripcion(HttpResponseException ex)
        {
            switch (ex.Response.StatusCode)
            {
                case HttpStatusCode.Unauthorized:
                    return Request.CreateErrorResponse(HttpStatusCode.Unauthorized, Configuracion.ObtenerMensajeTexto("UsuarioNoValido"));

                case HttpStatusCode.NotFound:
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, Configuracion.ObtenerMensajeTexto("TranscripcionNoEncontrada"));

                case HttpStatusCode.NoContent:
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, Configuracion.ObtenerMensajeTexto("TranscripcionPendiente"));

                case HttpStatusCode.InternalServerError:
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, Configuracion.ObtenerMensajeTexto("TranscripcionConErrores"));

                default:
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, Configuracion.ObtenerMensajeTexto("HaOcurridoUnError"));
            }
        }

        // [POST] api/Transcripciones
        public HttpResponseMessage PostTranscripcion()
        {
            try
            {
                string loginUsuario = new TranscripcionesBR().ObtenerUsuarioDeRequestYValidarAcceso(Request);
                new TranscripcionesBR().GrabarFicheroRecibido(bo, HttpContext.Current.Request, loginUsuario);
                return Request.CreateResponse(HttpStatusCode.Created, "La llamada se ha procesado con éxito");
            }
            catch (HttpResponseException ex)
            {
                return GestionarErrorPutTranscripcion(ex);
            }
        }

        private HttpResponseMessage GestionarErrorPutTranscripcion(HttpResponseException ex)
        {
            switch (ex.Response.StatusCode)
            {
                case HttpStatusCode.Unauthorized:
                    return Request.CreateErrorResponse(HttpStatusCode.Unauthorized, Configuracion.ObtenerMensajeTexto("UsuarioNoValido"));

                case HttpStatusCode.BadRequest:
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, Configuracion.ObtenerMensajeTexto("FicheroMp3NoEncontrado"));

                case HttpStatusCode.RequestEntityTooLarge:
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, Configuracion.ObtenerMensajeTexto("FicheroExcedeTamanyoMaximo"));

                case HttpStatusCode.UnsupportedMediaType:
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, Configuracion.ObtenerMensajeTexto("FormatoFicheroNoValido"));

                default:
                    return Request.CreateErrorResponse(HttpStatusCode.BadRequest, Configuracion.ObtenerMensajeTexto("HaOcurridoUnError"));

            }
        }

    }
}
