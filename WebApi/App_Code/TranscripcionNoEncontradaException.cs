using System;
using WebApi.Comun;

namespace WebApi.Negocio
{
    public class TranscripcionNoEncontradaException : Exception
    {
        public TranscripcionNoEncontradaException()
        {
        }

        public TranscripcionNoEncontradaException(string message)
            : base(message)
        {

        }

        public TranscripcionNoEncontradaException(string message, Exception inner)
            : base(message, inner)
        {

        }

        public override string Message
        {
            get
            {
                return Configuracion.ObtenerMensajeTexto("TranscripcionNoEncontrada");
            }
        }

    }



}