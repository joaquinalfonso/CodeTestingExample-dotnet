using System;
using WebApi.Comun;
using WebApi.Infraestructura;

namespace WebApi.Servicios
{

    // Excepción para notificar que la transcripción no se ha realizado todavía.

    public class TranscripcionPendienteException : Exception
    {
        public IConfiguracionResource configuracionResource { private get; set; }

        public TranscripcionPendienteException()
        {
            this.configuracionResource = new ConfiguracionResource();
        }

        public TranscripcionPendienteException(string message)
            : base(message)
        {
            this.configuracionResource = new ConfiguracionResource();
        }

        public TranscripcionPendienteException(string message, Exception inner)
            : base(message, inner)
        {
            this.configuracionResource = new ConfiguracionResource();
        }

        public override string Message
        {
            get
            {
                return configuracionResource.ObtenerConfiguracion().ObtenerMensajeTexto("TranscripcionPendiente");
            }
        }

    }



}