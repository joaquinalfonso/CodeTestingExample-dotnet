using System.Collections.Generic;
using WebApi.App_Code;
using WebApi.Models;

namespace WebApi.Controllers
{
    public interface ITranscripcionesBO
    {
        Transcripcion ObtenerTranscripcion(int id, string login);
        List<TranscripcionDTO> ObtenerTranscripciones(ParametrosGetTranscripcionesTO parametros);
        int ObtenerNuevoIdTranscripcion();
        void InsertarTranscripcion(Transcripcion transcripcion);
        void ActualizarTranscripcion(Transcripcion transcripcion);
        void ProcesarTranscripcion(Transcripcion transcripcion);
        void ProcesarTranscripcionesPendientes();
        void ProcesarTranscripciones(List<Transcripcion> transcripciones);
    }
}
