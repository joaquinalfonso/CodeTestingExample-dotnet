using System;
using WebApi.Models;

namespace WebApi.Comun
{
    
    // Data Transfer Object para enviar un subconjunto de los datos del modelo.

    public class TranscripcionDTO
    {
        public int Id { get; set; }
        public string FechaRecepcion { get; set; }
        public string NombreFicheroMp3 { get; set; }
        public string Estado { get; set; }
        public string FechaTranscripcion { get; set; }

        public TranscripcionDTO(Transcripcion transcripcion)
        {
            this.Id = transcripcion.Id;
            this.FechaRecepcion = transcripcion.FechaHoraRecepcion.ToShortDateString();
            this.NombreFicheroMp3 = transcripcion.NombreArchivo;
            this.Estado = transcripcion.Estado;
            this.FechaTranscripcion = (transcripcion.FechaHoraTranscripcion != null) ? ((DateTime)transcripcion.FechaHoraTranscripcion).ToShortDateString() : "";
        }
    }
}