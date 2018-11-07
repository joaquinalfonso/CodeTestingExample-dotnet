namespace WebApi.Infraestructura
{
    // Clase para definir constantes del programa. 
    // Sería mejor almacenarlo en una tabla, un archivo de recursos o de configuración, etc..
    // y se podría cambiar sin necesidad de hacer una release nueva 

    public class Configuracion
    {
        public int NUMERO_HILOS_PROCESAMIENTO_TRANSCRIPCIONES = 3;
        public string RUTA_FICHEROS_TRANSCRITOS = "~/App_Data/TranscripcionesTxt/";
        public string RUTA_FICHEROS_MP3 = "~/App_Data/UploadMp3/";
        public int TAMANYO_MAX_BYTES_MP3 = 5242880;   // 5 MB
        public string EXTENSION_FICHEROS_AUDIO = ".MP3";
        public string FORMATO_FECHA_VARIABLE_QUERYSTRING = "yyyy-MM-ddTHH:mm";

        public string ObtenerMensajeTexto(string clave)
        {
            //Se puede  Obtener de un archivo de recursos o un servicio

            return clave;
        }
    }

}