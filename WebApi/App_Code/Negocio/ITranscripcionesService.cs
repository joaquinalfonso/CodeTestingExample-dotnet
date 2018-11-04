using System.Collections.Generic;
using System.Web;
using WebApi.Comun;
using WebApi.Models;

namespace WebApi.Negocio
{
    public interface ITranscripcionesService
    {

        List<TranscripcionDTO> ObtenerTranscripciones(ParametrosGetTranscripcionesTO parametros);
        int ObtenerNuevoIdTranscripcion();
        //void InsertarTranscripcion(Transcripcion transcripcion);
        //void ActualizarTranscripcion(Transcripcion transcripcion);
        void ProcesarTranscripcion(Transcripcion transcripcion);
        void ProcesarTranscripcionesPendientes();
        void ProcesarTranscripciones(List<Transcripcion> transcripciones);
        string ObtenerTextoTranscripcionRealizada(int id, string login);
        void RecibirFicheroATranscribir(HttpPostedFile fichero, string login);

    }
}
