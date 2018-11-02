using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApiCrud.Models;

namespace WebApiCrud.App_Code
{
    public class TranscripcionDTO
    {
        public string Id { get; set; }
        public string FechaRecepcion { get; set; }
        public string NombreFicheroMp3 { get; set; }
        public string Estado { get; set; }
        public string FechaTranscripcion { get; set; }

        public TranscripcionDTO(Transcripciones transcripcion)
        {
            this.Id = transcripcion.Id;
            this.FechaRecepcion = transcripcion.FechaHoraRecepcion.ToShortDateString();
            this.NombreFicheroMp3 = transcripcion.NombreArchivo;
            this.Estado = transcripcion.Estado;
            this.FechaTranscripcion = (transcripcion.FechaHoraTranscripcion != null) ? ((DateTime)transcripcion.FechaHoraTranscripcion).ToShortDateString() : "";
        }
    }
}