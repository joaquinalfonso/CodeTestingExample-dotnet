using System;
using WebApi.Comun;

namespace WebApi.Servicios
{

    // Excepción para notificar que la transcripción ha sido erronea.

    public class TranscripcionErroneaException : Exception
    {
        public TranscripcionErroneaException()
        {
        }

        public TranscripcionErroneaException(string message)
            : base(message)
        {

        }

        public TranscripcionErroneaException(string message, Exception inner)
            : base(message, inner)
        {

        }

        public override string Message
        {
            get
            {
                return Configuracion.ObtenerMensajeTexto("TranscripcionConErrores");
            }
        }

    }



}