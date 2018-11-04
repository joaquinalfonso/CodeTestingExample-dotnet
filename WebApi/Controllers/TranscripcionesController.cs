using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using WebApi.Comun;
using WebApi.Servicios;

namespace WebApi.Controllers
{
    public class TranscripcionesController : ApiController
    {
        public ITranscripcionesService transcripcionesService { private get; set; }

        public TranscripcionesController()
        {
            this.transcripcionesService = new TranscripcionesService();
        }
        
        #region Caso de uso 1 (CU1)

        // Recepción de un fichero para transcribir.
        // Identificador: CU1.
        // [POST] api/Transcripciones

        public async Task<HttpResponseMessage> PostTranscripcion()
        {
            try
            {
                string loginUsuario = new TranscripcionesControllerValidaciones().ObtenerUsuarioDeRequestYValidarAcceso(Request);
                HttpPostedFile ficheroRecibido = new TranscripcionesControllerValidaciones().ValidarYExtraerFicheroRecibido(HttpContext.Current.Request, loginUsuario);
                transcripcionesService.RecibirFicheroATranscribir(ficheroRecibido, loginUsuario);
                return Request.CreateResponse(HttpStatusCode.Created, "La llamada se ha procesado con éxito");
            }
            catch (Exception ex)
            {
                return GestionarErrorPutTranscripcion(ex);
            }
        }

        private HttpResponseMessage GestionarErrorPutTranscripcion(Exception ex)
        {
            //TODO
            if (ex.GetType().IsAssignableFrom(typeof(HttpResponseException)))
            {
                switch (((HttpResponseException)ex).Response.StatusCode)
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
            else
            if (ex.GetType().IsAssignableFrom(typeof(TranscripcionNoGuardadaException)))
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message);
            else
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, Configuracion.ObtenerMensajeTexto("HaOcurridoUnError"));
        }

        #endregion

        #region Caso de uso 2 (CU2)

        // Consulta del estado de transcripciones para un usuario.
        // Identificador: CU2.

        // [GET] api/Transcripciones
        // [GET] api/Transcripciones?desde=yyyy-MM-ddTHH:mm
        // [GET] api/Transcripciones?hasta=yyyy-MM-ddTHH:mm
        // [GET] api/Transcripciones?desde=yyyy-MM-ddTHH:mm&hasta=yyyy-MM-ddTHH:mm

        public HttpResponseMessage GetTranscripciones(string desde = "", string hasta = "")
        {
            try
            {
                ParametrosConsultaTranscripcionesTO parametrosConsulta = new TranscripcionesControllerValidaciones().ObtenerYValidarParametrosConsultaGetTranscripciones(Request, desde, hasta);

                List<TranscripcionDTO> listaTranscripciones = transcripcionesService.ObtenerTranscripciones(parametrosConsulta);

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

        #endregion

        #region Caso de uso 3 (CU3)

        // Envío del resultado de una transcripción.
        // Identificador: CU3.

        // [GET] api/Transcripciones/{id}

        public HttpResponseMessage GetTranscripcion(int id)
        {
            try
            {
                string loginUsuario = new TranscripcionesControllerValidaciones().ObtenerUsuarioDeRequestYValidarAcceso(Request);

                string textoTranscrito = transcripcionesService.ObtenerTextoTranscripcionRealizada(id, loginUsuario);

                return Request.CreateResponse(HttpStatusCode.OK, textoTranscrito);
            }
            catch (Exception ex)
            {
                return GestionarErrorGetTranscripcion(ex);
            }

        }

        private HttpResponseMessage GestionarErrorGetTranscripcion(Exception ex)
        {
            if (ex.GetType().IsAssignableFrom(typeof(HttpResponseException))
                &&
                ((HttpResponseException)ex).Response.StatusCode == HttpStatusCode.Unauthorized)
                return Request.CreateErrorResponse(HttpStatusCode.Unauthorized, Configuracion.ObtenerMensajeTexto("UsuarioNoValido"));
            else
            if (ex.GetType().IsAssignableFrom(typeof(TranscripcionNoEncontradaException)))
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, ex.Message);
            else
            if (ex.GetType().IsAssignableFrom(typeof(TranscripcionPendienteException)))
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message);
            else
            if (ex.GetType().IsAssignableFrom(typeof(TranscripcionErroneaException)))
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message);
            else
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, Configuracion.ObtenerMensajeTexto("HaOcurridoUnError"));
        }

        #endregion

    }
}
