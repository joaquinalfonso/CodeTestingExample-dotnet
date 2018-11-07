using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using WebApi.Comun;
using WebApi.Servicios;
using WebApi.Infraestructura;

namespace WebApi.Controllers
{

    // Clase que implementa las validaciones de entrada de datos del servicio REST.

    public class TranscripcionesControllerValidaciones
    {

        public IConfiguracionResource configuracionResource { private get; set; }

        public TranscripcionesControllerValidaciones()
        {
            this.configuracionResource = new ConfiguracionResource();
        }


        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        private string ObtenerUsuarioDeRequest(HttpRequestMessage request)
        {

            string loginUsuario = "";

            try
            {
                if (request.Headers.Contains("Login"))
                    loginUsuario = request.Headers.GetValues("Login").First();
            }
            catch
            {
                throw new Exception("Login no definido");
            }

            return loginUsuario;

        }

        public string ObtenerUsuarioDeRequestYValidarAcceso(HttpRequestMessage request)
        {
            string loginUsuario = "";
            bool esUsuarioValido = false;

            try
            {
                loginUsuario = ObtenerUsuarioDeRequest(request);
                esUsuarioValido = new UsuarioService().EsUsuarioValido(loginUsuario);
            }
            catch
            {
                throw new HttpResponseException(HttpStatusCode.Unauthorized);
            }
            finally
            {
                if (!esUsuarioValido)
                    throw new HttpResponseException(HttpStatusCode.Unauthorized);
            }
            return loginUsuario;
        }

        public ParametrosConsultaTranscripcionesTO ObtenerYValidarParametrosConsultaGetTranscripciones(HttpRequestMessage request, string desde, string hasta)
        {
            ParametrosConsultaTranscripcionesTO parametros = new ParametrosConsultaTranscripcionesTO();

            //comprobar que los nombres de parametros son "desde o hasta"
            var queryString = request.GetQueryNameValuePairs().ToDictionary(x => x.Key, x => x.Value).ToList();
            queryString.ForEach(x =>
            {
                if (x.Key.ToUpper() != "DESDE" && x.Key.ToUpper() != "HASTA")
                    throw new HttpResponseException(HttpStatusCode.BadRequest);
            }
            );

            try
            {

                parametros.Login = ObtenerUsuarioDeRequestYValidarAcceso(request);

                if (desde != "")
                    parametros.FechaDesde = DateTime.ParseExact(desde, configuracionResource.ObtenerConfiguracion().FORMATO_FECHA_VARIABLE_QUERYSTRING, System.Globalization.CultureInfo.InvariantCulture);
                if (hasta != "")
                    parametros.FechaHasta = DateTime.ParseExact(hasta, configuracionResource.ObtenerConfiguracion().FORMATO_FECHA_VARIABLE_QUERYSTRING, System.Globalization.CultureInfo.InvariantCulture);

                return parametros;

            }
            catch (HttpResponseException)
            {
                throw;
            }
            catch
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }
        }

        private void ValidarTipoFichero(HttpPostedFile fichero)
        {
            if (!Path.GetExtension(fichero.FileName).ToUpper().Equals(configuracionResource.ObtenerConfiguracion().EXTENSION_FICHEROS_AUDIO))
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
        }

        private void ValidarTamanyoFichero(HttpPostedFile fichero)
        {
            if (fichero.ContentLength > configuracionResource.ObtenerConfiguracion().TAMANYO_MAX_BYTES_MP3)
                throw new HttpResponseException(HttpStatusCode.RequestEntityTooLarge);
        }
       
        public HttpPostedFile ValidarYExtraerFicheroRecibido(HttpRequest request, string loginUsuario)
        {

            if (request.Files.Count != 1)
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }

            var ficheroEnviado = request.Files["mp3"];

            if (ficheroEnviado == null)
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }

            ValidarTipoFichero(ficheroEnviado);
            ValidarTamanyoFichero(ficheroEnviado);

            return ficheroEnviado;

            
        }
       
    }


}