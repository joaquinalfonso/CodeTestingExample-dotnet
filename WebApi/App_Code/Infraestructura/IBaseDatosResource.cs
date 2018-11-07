using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApi.Comun;
using WebApi.Models;

namespace WebApi.Infraestructura
{
    public interface IBaseDatosResource
    {
        //int ObtenerNuevoIdTranscripcion();
        List<Transcription> ObtenerTranscripcionesPendientes();
        List<TranscripcionDTO> ObtenerTranscripciones(ParametrosConsultaTranscripcionesTO parametros);
        Transcription ObtenerTranscripcion(int id, string login);
        void ActualizarTranscripcion(Transcription transcripcion);
        int InsertarTranscripcion(Transcription transcripcion);

    }
}
