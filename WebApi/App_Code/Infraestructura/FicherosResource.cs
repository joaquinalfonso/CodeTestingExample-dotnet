using System.IO;
using System.Web;
using WebApi.Comun;

namespace WebApi.Infraestructura
{

    // Clase de recursos que implementa la capa de operaciones de almacenamiento en disco.
    // Implementa la interface IFicherosResource

    public class FicherosResource : IFicherosResource
    {

        public IConfiguracionResource configuracionResource { private get; set; }

        public FicherosResource()
        {
            this.configuracionResource = new ConfiguracionResource();
        }



        public bool ExisteFicheroTranscritoTxt(string nombreFichero)
        {
            string rutaFichero = ObtenerRutaFicheroTranscritoTxt(nombreFichero);
            return File.Exists(rutaFichero);
        }

        public void GrabarFicheroTextoTranscrito(string nombreFichero, string textoTranscrito)
        {
            string rutaFicheroTranscrito = ObtenerRutaFicheroTranscritoTxt(nombreFichero);
            File.WriteAllText(rutaFicheroTranscrito, textoTranscrito, System.Text.Encoding.UTF8);
        }

        private string ObtenerRutaFicheroTranscritoTxt(string nombreFichero)
        {
            string rutaFicherosTranscripciones = System.Web.Hosting.HostingEnvironment.MapPath(configuracionResource.ObtenerConfiguracion().RUTA_FICHEROS_TRANSCRITOS);
            return rutaFicherosTranscripciones + nombreFichero;
        }

        private string ObtenerRutaFicheroMp3(string nombreFichero)
        {
            string rutaFicherosMp3 = System.Web.Hosting.HostingEnvironment.MapPath(configuracionResource.ObtenerConfiguracion().RUTA_FICHEROS_MP3);
            return string.Format("{0}{1}", rutaFicherosMp3, nombreFichero);
        }

        public byte[] ObtenerFicheroMp3(string nombreFichero)
        {
            string rutaFicheroMp3 = ObtenerRutaFicheroMp3(nombreFichero);

            byte[] ficheroMp3 = File.ReadAllBytes(rutaFicheroMp3);

            return ficheroMp3;
        }

        public string ObtenerFicheroTranscritoTxt(string nombreFichero)
        {
            string rutaFicheroTxt = ObtenerRutaFicheroTranscritoTxt(nombreFichero);

            string contenidoFicheroTxt = "";
            contenidoFicheroTxt = File.ReadAllText(rutaFicheroTxt, System.Text.Encoding.UTF8);

            return contenidoFicheroTxt;
        }

        public void GrabarFicheroMp3(HttpPostedFile postedFile, string nombreFichero)
        {
            string rutaGuardado = HttpContext.Current.Server.MapPath(configuracionResource.ObtenerConfiguracion().RUTA_FICHEROS_MP3);
            var filePath = rutaGuardado + nombreFichero;
            postedFile.SaveAs(filePath);
        }
    }
}