using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApi.Comun;
using WebApi.Infraestructura;
using WebApi.Models;

namespace EjercicioSeleccionVocali.Tests.Mocks
{
    public class BaseDatosResourceMock : IBaseDatosResource
    {
        public void ActualizarTranscripcion(Transcription transcripcion)
        {
            throw new NotImplementedException();
        }

        public int InsertarTranscripcion(Transcription transcripcion)
        {
            throw new NotImplementedException();
        }

        public Transcription ObtenerTranscripcion(int id, string login)
        {
            throw new NotImplementedException();
        }

        public List<TranscripcionDTO> ObtenerTranscripciones(ParametrosConsultaTranscripcionesTO parametros)
        {
            throw new NotImplementedException();
        }

        public List<Transcription> ObtenerTranscripcionesPendientes()
        {
            throw new NotImplementedException();
        }
    }
}
