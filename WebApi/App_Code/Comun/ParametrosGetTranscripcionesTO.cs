using System;

namespace WebApi.Comun
{
    // Transfer Object para pasar los parámetros de consulta de las transcripción.

    public class ParametrosConsultaTranscripcionesTO
    {
        public string Login { get; set; }
        public DateTime? FechaDesde { get; set; }
        public DateTime? FechaHasta { get; set; }
    }

}