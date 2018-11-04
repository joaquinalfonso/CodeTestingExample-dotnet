using System;
using WebApi.Comun;

namespace WebApi.Servicios
{

    // Excepción para notificar que la transcripción no se ha realizado todavía.

    public class TranscripcionPendienteException : Exception
    {
        public TranscripcionPendienteException()
        {
        }

        public TranscripcionPendienteException(string message)
            : base(message)
        {

        }

        public TranscripcionPendienteException(string message, Exception inner)
            : base(message, inner)
        {

        }

        public override string Message
        {
            get
            {
                return Configuracion.ObtenerMensajeTexto("TranscripcionPendiente");
            }
        }

    }



}