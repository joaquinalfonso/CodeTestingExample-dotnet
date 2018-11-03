using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using WebApi.Comun;

namespace WebApi.App_Code
{
    public class FicherosBR
    {

        private string ObtenerRutaFicheroTranscritoTxt(int id)
        {
            string rutaFicherosTranscripciones = System.Web.Hosting.HostingEnvironment.MapPath(Configuracion.RUTA_FICHEROS_TRANSCRITOS);
            var rutaFicheroTxt = rutaFicherosTranscripciones + string.Format("{0}.txt", id);
            return rutaFicheroTxt;
        }

        public bool ExisteFicheroTranscritoTxt(int id)
        {
            string rutaFicheroTxt = ObtenerRutaFicheroTranscritoTxt(id);
            return File.Exists(rutaFicheroTxt);
        }

        public string ObtenerFicheroTranscritoTxt(int id)
        {
            string rutaFicheroTxt = ObtenerRutaFicheroTranscritoTxt(id);

            string contenidoFicheroTxt = "";
            contenidoFicheroTxt = File.ReadAllText(rutaFicheroTxt, System.Text.Encoding.UTF8);
            //using (StreamReader sr = new StreamReader(rutaFicheroTxt, System.Text.Encoding.Default, false))
            //{
            //    contenidoFicheroTxt = sr.ReadToEnd().Replace("\r\n", " ");
            //}

            return contenidoFicheroTxt;
        }

        public void GrabarFicheroTextoTranscrito(int id, string textoTranscrito)
        {
            string rutaFicheroTranscrito = ObtenerRutaFicheroTranscritoTxt(id);
            File.WriteAllText(rutaFicheroTranscrito, textoTranscrito);
        }
    }
}