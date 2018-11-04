using System;
using WebApi.Comun;

namespace WebApi.Negocio
{
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