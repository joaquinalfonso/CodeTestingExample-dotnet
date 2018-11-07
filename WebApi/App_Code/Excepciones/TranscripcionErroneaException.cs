using System;
using WebApi.Comun;
using WebApi.Infraestructura;

namespace WebApi.Servicios
{

    // Excepción para notificar que la transcripción ha sido erronea.

    public class TranscripcionErroneaException : Exception
    {
        public IConfiguracionResource configuracionResource { private get; set; }

        public TranscripcionErroneaException()
        {
            this.configuracionResource = new ConfiguracionResource();
        }

        public TranscripcionErroneaException(string message)
            : base(message)
        {
            this.configuracionResource = new ConfiguracionResource();
        }

        public TranscripcionErroneaException(string message, Exception inner)
            : base(message, inner)
        {
            this.configuracionResource = new ConfiguracionResource();
        }

        public override string Message
        {
            get
            {
                return configuracionResource.ObtenerConfiguracion().ObtenerMensajeTexto("TranscripcionConErrores");
            }
        }

    }



}