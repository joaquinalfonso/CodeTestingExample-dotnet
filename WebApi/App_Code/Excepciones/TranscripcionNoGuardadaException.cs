using System;
using WebApi.Comun;

namespace WebApi.Servicios
{

    // Excepción para notificar que ha ocurrido un error al guardar la transcripción.

    public class TranscripcionNoGuardadaException : Exception
    {
        public TranscripcionNoGuardadaException()
        {
        }

        public TranscripcionNoGuardadaException(string message)
            : base(message)
        {

        }

        public TranscripcionNoGuardadaException(string message, Exception inner)
            : base(message, inner)
        {

        }

        public override string Message
        {
            get
            {
                return Configuracion.ObtenerMensajeTexto("TranscripcionNoGuardada");
            }
        }

    }



}