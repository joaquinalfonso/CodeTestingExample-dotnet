using System;

namespace WebApi.App_Code
{
    public class ParametrosGetTranscripcionesTO
    {
        public string Login { get; set; }
        public DateTime? FechaDesde { get; set; }
        public DateTime? FechaHasta { get; set; }
    }

}