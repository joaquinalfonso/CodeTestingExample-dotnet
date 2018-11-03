using System;

namespace WebApi.App_Code
{
    public class ParametrosGetTranscripcionesTO
    {
        public string Login { get; set; }
        public DateTime? Desde { get; set; }
        public DateTime? Hasta { get; set; }
    }

}