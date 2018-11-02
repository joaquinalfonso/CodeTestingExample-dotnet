using System.Collections.Generic;
using WebApiCrud.App_Code;
using WebApiCrud.Models;

namespace WebApiCrud.Controllers
{
    public interface ITranscripcionesBO
    {
        Transcripciones ObtenerTranscripcion(string id, string login);
        List<TranscripcionDTO> ObtenerTranscripciones(ParametrosGetTranscripcionesTO parametros);
        void InsertarTranscripcion(Transcripciones transcripcion);
        void ActualizarTranscripcion(Transcripciones transcripcion);
        void ProcesarTranscripcion(Transcripciones transcripcion);
        bool ExisteFicheroTranscritoTxt(Transcripciones transcripcion);
        string ObtenerFicheroTranscritoTxt(Transcripciones transcripcion);
        void ProcesarTranscripcionesPendientes();
        void ProcesarTranscripciones(List<Transcripciones> transcripciones);
    }
}
