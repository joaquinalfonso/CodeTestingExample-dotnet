using System;
using System.Collections.Generic;
using System.Web;
using WebApi.Comun;
using WebApi.Models;
using WebApi.Negocio;

namespace EjercicioSeleccionVocali.Tests.Mocks
{
    public class TranscripcionesBOMock : ITranscripcionesBO
    {
        private List<Transcripcion> testTranscripciones = new List<Transcripcion>();

        public TranscripcionesBOMock()
        {
            
            testTranscripciones.Add(new Transcripcion { Id = 1, FechaHoraRecepcion = DateTime.Now, LoginUsuario = "Usuario1", NombreArchivo = "Archivo1.mp3", Estado = TipoEstadoTranscripcion.PENDIENTE.ToString() });
            testTranscripciones.Add(new Transcripcion { Id = 2, FechaHoraRecepcion = DateTime.Now.AddDays(-1), LoginUsuario = "Usuario1", NombreArchivo = "Archivo2.mp3", Estado = TipoEstadoTranscripcion.PENDIENTE.ToString() });
            testTranscripciones.Add(new Transcripcion { Id = 3, FechaHoraRecepcion = DateTime.Now.AddDays(-2), LoginUsuario = "Usuario2", NombreArchivo = "Archivo3.mp3", Estado = TipoEstadoTranscripcion.PENDIENTE.ToString() });

        }

        public void ActualizarTranscripcion(Transcripcion transcripcion)
        {
            throw new NotImplementedException();
        }

        public void InsertarTranscripcion(Transcripcion transcripcion)
        {
            throw new NotImplementedException();
        }

        public int ObtenerNuevoIdTranscripcion()
        {
            throw new NotImplementedException();
        }

        public Transcripcion ObtenerTranscripcionRealizada(int id, string login)
        {
            throw new NotImplementedException();
        }

        public List<TranscripcionDTO> ObtenerTranscripciones(ParametrosGetTranscripcionesTO parametros)
        {
            List<TranscripcionDTO> listaTranscripciones = new List<TranscripcionDTO>();
            testTranscripciones.ForEach(x => listaTranscripciones.Add(new TranscripcionDTO(x)));

            return listaTranscripciones;
        }

        public void ProcesarTranscripcion(Transcripcion transcripcion)
        {
            throw new NotImplementedException();
        }

        public void ProcesarTranscripciones(List<Transcripcion> transcripciones)
        {
            throw new NotImplementedException();
        }

        public void ProcesarTranscripcionesPendientes()
        {
            throw new NotImplementedException();
        }

        public string ObtenerFicheroTranscritoTxt(Transcripcion transcripcion)
        {
            throw new NotImplementedException();
        }

        public void RecibirFicheroATranscribir(HttpPostedFile fichero, string login)
        {
            throw new NotImplementedException();
        }
    }
}
