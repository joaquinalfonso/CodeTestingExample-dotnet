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
        int ObtenerNuevoIdTranscripcion();
        List<Transcripcion> ObtenerTranscripcionesPendientes();
        List<TranscripcionDTO> ObtenerTranscripciones(ParametrosConsultaTranscripcionesTO parametros);
        Transcripcion ObtenerTranscripcion(int id, string login);
        void ActualizarTranscripcion(Transcripcion transcripcion);
        void InsertarTranscripcion(Transcripcion transcripcion);

    }
}
