using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using WebApi.Comun;
using WebApi.App_Code;
using WebApi.Models;

namespace WebApi.Controllers
{

    public class TranscripcionesBR
    {

        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();

        public string ObtenerUsuarioDeRequest(HttpRequestMessage request)
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
                esUsuarioValido = new UsuarioBR().EsUsuarioValido(loginUsuario);
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

        public ParametrosGetTranscripcionesTO ObtenerParametrosConsultaGetTranscripciones(HttpRequestMessage request, string desde, string hasta)
        {
            ParametrosGetTranscripcionesTO parametros = new ParametrosGetTranscripcionesTO();

            //TODO: comprobar que estan los nombres de parametros "desde o hasta"
            var queryString = request.GetQueryNameValuePairs();

            try
            {

                if (desde != "")
                    parametros.Desde = DateTime.ParseExact(desde, Configuracion.FORMATO_FECHA_VARIABLE_QUERYSTRING, System.Globalization.CultureInfo.InvariantCulture);
                if (hasta != "")
                    parametros.Hasta = DateTime.ParseExact(hasta, Configuracion.FORMATO_FECHA_VARIABLE_QUERYSTRING, System.Globalization.CultureInfo.InvariantCulture);

                return parametros;

            }
            catch (Exception ex)
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }
        }

        public void GrabarFicheroMp3(HttpPostedFile postedFile, int idTranscripcion)
        {
            string rutaGuardado = HttpContext.Current.Server.MapPath(Configuracion.RUTA_FICHEROS_MP3);
            var filePath = rutaGuardado + string.Format("{0}.mp3", idTranscripcion);

            postedFile.SaveAs(filePath);
        }

        public void ValidarFichero(HttpPostedFile fichero)
        {
            ValidarTipoFichero(fichero);
            ValidarTamanyoFichero(fichero);
        }

        private void ValidarTipoFichero(HttpPostedFile fichero)
        {
            // TODO : Validar que el formato del fichero es mp3
            if (!Path.GetExtension(fichero.FileName).ToUpper().Equals(Configuracion.EXTENSION_FICHEROS_AUDIO))
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
        }

        private void ValidarTamanyoFichero(HttpPostedFile fichero)
        {
            if (fichero.ContentLength > Configuracion.TAMANYO_MAX_BYTES_MP3)
                throw new HttpResponseException(HttpStatusCode.RequestEntityTooLarge);
        }

        public string ObtenerNuevoIdTranscripcion()
        {
            int tokenSize = Configuracion.NUMERO_CARACTERES_TRANSCRIPCIONES_ID;

            string token = ObtenerCadenaAlfanumericaAleatoria(tokenSize);

            return token;
        }

        private string ObtenerCadenaAlfanumericaAleatoria(int longitud)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";

            Random random = new Random();

            return new string(Enumerable.Repeat(chars, longitud)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public Transcripcion ObtenerTranscripcionRealizada(ITranscripcionesBO bo, int id, string login)
        {
            Transcripcion transcripcion = bo.ObtenerTranscripcion(id, login);

            // Flujo Alternativo A
            if (transcripcion == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            // Flujo Alternativo B
            if (transcripcion.Estado == TipoEstadoTranscripcion.PENDIENTE.ToString() ||
                transcripcion.Estado == TipoEstadoTranscripcion.EN_PROGRESO.ToString())
            {
                throw new HttpResponseException(HttpStatusCode.NoContent);
            }

            // Flujo Alternativo C
            if (transcripcion.Estado == TipoEstadoTranscripcion.ERROR.ToString())
            {
                throw new HttpResponseException(HttpStatusCode.InternalServerError);
            }

            return transcripcion;
        }

        public string ObtenerFicheroTranscritoTxt(Transcripcion transcripcion)
        {

            string texto = "";

            if (transcripcion == null)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            if (transcripcion.Estado != TipoEstadoTranscripcion.REALIZADA.ToString())
            {
                throw new HttpResponseException(HttpStatusCode.NoContent);
            }

            try
            {
                if (new FicherosBR().ExisteFicheroTranscritoTxt(transcripcion.Id))
                {
                    texto = new FicherosBR().ObtenerFicheroTranscritoTxt(transcripcion.Id);
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex, ex.Message);
                throw new FileNotFoundException();
            }

            if (String.IsNullOrEmpty(texto))
            {
                throw new HttpResponseException(HttpStatusCode.NoContent);
            }

            return texto;

        }


        //public static bool ValidarFicheroImagen(string filename, ref string mensaje)
        //{
        //    bool resultado = false;
        //    mensaje = "";

        //    // DICTIONARY OF ALL IMAGE FILE HEADER
        //    Dictionary<string, byte[]> imageHeader = new Dictionary<string, byte[]>();
        //    imageHeader.Add("JPG", new byte[] { 0xFF, 0xD8, 0xFF, 0xE0 });
        //    imageHeader.Add("JPEG", new byte[] { 0xFF, 0xD8, 0xFF, 0xE0 });
        //    imageHeader.Add("PNG", new byte[] { 0x89, 0x50, 0x4E, 0x47 });
        //    imageHeader.Add("TIF", new byte[] { 0x49, 0x49, 0x2A, 0x00 });
        //    imageHeader.Add("TIFF", new byte[] { 0x49, 0x49, 0x2A, 0x00 });
        //    imageHeader.Add("GIF", new byte[] { 0x47, 0x49, 0x46, 0x38 });
        //    imageHeader.Add("BMP", new byte[] { 0x42, 0x4D });
        //    imageHeader.Add("ICO", new byte[] { 0x00, 0x00, 0x01, 0x00 });

        //    byte[] header;

        //    if (File.Exists(filename))
        //    {
        //        // GET FILE EXTENSION
        //        string fileExt;
        //        fileExt = filename.Substring(filename.LastIndexOf('.') + 1).ToUpper();

        //        // CUSTOM VALIDATION GOES HERE BASED ON FILE EXTENSION IF ANY

        //        byte[] tmp = imageHeader[fileExt];
        //        header = new byte[tmp.Length];

        //        // GET HEADER INFORMATION OF UPLOADED FILE

        //        FileStream fs = new FileStream(filename, FileMode.Open, FileAccess.Read);
        //        BinaryReader br = new BinaryReader(fs);
        //        header = br.ReadBytes(header.Length);

        //        //            File.Create(filename).Read(header, 0, header.Length);

        //        if (CompareArray(tmp, header))
        //        {
        //            resultado = true;
        //            mensaje = "Valid ." + fileExt + " file.";
        //            // VALID HEADER INFORMATION 
        //            // CODE TO PROCESS FILE
        //        }
        //        else
        //        {
        //            mensaje = "Invalid ." + fileExt + " file.";
        //            // INVALID HEADER INFORMATION
        //        }
        //    }
        //    else
        //    {
        //        mensaje = "Please select image file.";
        //    }


        //    return resultado;
        //}

    }


}