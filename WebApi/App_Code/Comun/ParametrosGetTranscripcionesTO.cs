using System;

namespace WebApi.Comun
{
    public class ParametrosGetTranscripcionesTO
    {
        public string Login { get; set; }
        public DateTime? FechaDesde { get; set; }
        public DateTime? FechaHasta { get; set; }
    }

}