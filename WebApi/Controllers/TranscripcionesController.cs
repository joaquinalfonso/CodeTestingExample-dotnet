﻿using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using WebApi.Comun;
using WebApi.Models;
using WebApi.Negocio;

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

        // [GET] api/Transcripciones
        // [GET] api/Transcripciones?desde=yyyy-MM-ddTHH:mm
        // [GET] api/Transcripciones?hasta=yyyy-MM-ddTHH:mm
        // [GET] api/Transcripciones?desde=yyyy-MM-ddTHH:mm&hasta=yyyy-MM-ddTHH:mm
        public HttpResponseMessage GetTranscripciones(string desde = "", string hasta = "")
        {
            try
            {
                ParametrosGetTranscripcionesTO parametrosConsulta = new TranscripcionesBR().ObtenerYValidarParametrosConsultaGetTranscripciones(Request, desde, hasta);

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
                    return Request.CreateErrorResponse(HttpStatusCode.Unauthorized, 
                        //"Usuario no valido");
                        Configuracion.ObtenerMensajeTexto("UsuarioNoValido"));

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
                Transcripcion transcripcion = bo.ObtenerTranscripcionRealizada(id, loginUsuario);
                string textoTranscrito = bo.ObtenerFicheroTranscritoTxt(transcripcion);

                return Request.CreateResponse(HttpStatusCode.OK, textoTranscrito);
            }
            catch (Exception ex)
            {
                return GestionarErrorGetTranscripcion(ex);
            }

        }

        private HttpResponseMessage GestionarErrorGetTranscripcion(Exception ex)
        {
            if (ex.GetType().IsAssignableFrom(typeof(HttpResponseException)) &&
               ((HttpResponseException)ex).Response.StatusCode == HttpStatusCode.Unauthorized)
                return Request.CreateErrorResponse(HttpStatusCode.Unauthorized, Configuracion.ObtenerMensajeTexto("UsuarioNoValido"));
            else if (ex.GetType().IsAssignableFrom(typeof(TranscripcionNoEncontradaException)))
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, ex.Message);
            else if (ex.GetType().IsAssignableFrom(typeof(TranscripcionPendienteException)))
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message);
            else if (ex.GetType().IsAssignableFrom(typeof(TranscripcionErroneaException)))
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex.Message);
            else
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, Configuracion.ObtenerMensajeTexto("HaOcurridoUnError"));
        }

        // [POST] api/Transcripciones
        public HttpResponseMessage PostTranscripcion()
        {
            try
            {
                string loginUsuario = new TranscripcionesBR().ObtenerUsuarioDeRequestYValidarAcceso(Request);
                HttpPostedFile ficheroRecibido = new TranscripcionesBR().ValidarYExtraerFicheroRecibido(HttpContext.Current.Request, loginUsuario);
                bo.RecibirFicheroATranscribir(ficheroRecibido, loginUsuario);
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