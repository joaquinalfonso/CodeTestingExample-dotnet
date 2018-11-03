namespace WebApi.Comun
{
    public static class Configuracion
    {
        public const int NUMERO_HILOS_PROCESAMIENTO_TRANSCRIPCIONES = 3;
        public const string RUTA_FICHEROS_TRANSCRITOS = "~/App_Data/TranscripcionesTxt/";
        public const string RUTA_FICHEROS_MP3 = "~/App_Data/UploadMp3/";
        public const int TAMANYO_MAX_BYTES_MP3 = 5242880;   // 5 MB
        public const string EXTENSION_FICHEROS_AUDIO = ".MP3";
        public const int NUMERO_CARACTERES_TRANSCRIPCIONES_ID = 10;
        public const string FORMATO_FECHA_VARIABLE_QUERYSTRING = "yyyy-MM-ddTHH:mm";
    }
}