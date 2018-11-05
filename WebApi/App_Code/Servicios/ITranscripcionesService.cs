using System.Collections.Generic;
using System.Web;
using WebApi.Comun;
using WebApi.Models;

namespace WebApi.Servicios
{

    // Interface que define las operaciones del servicio de transcripciones.

    public interface ITranscripcionesService
    {
        List<TranscripcionDTO> ObtenerTranscripciones(ParametrosConsultaTranscripcionesTO parametros);
        //int ObtenerNuevoIdTranscripcion();
        void ProcesarTranscripcion(Transcripcion transcripcion);
        void ProcesarTranscripcionesPendientes();
        //void ProcesarTranscripciones(List<Transcripcion> transcripciones);
        string ObtenerTextoTranscripcionRealizada(int id, string login);
        void RecibirFicheroATranscribir(HttpPostedFile fichero, string login);

    }
}
