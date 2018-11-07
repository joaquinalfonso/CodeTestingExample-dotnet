using System;
using WebApi.Comun;
using WebApi.Infraestructura;

namespace WebApi.Servicios
{

    // Excepción para notificar que la transcripción no existe.

    public class TranscripcionNoEncontradaException : Exception
    {
        public IConfiguracionResource configuracionResource { private get; set; }

        public TranscripcionNoEncontradaException()
        {
            this.configuracionResource = new ConfiguracionResource();
        }

        public TranscripcionNoEncontradaException(string message)
            : base(message)
        {
            this.configuracionResource = new ConfiguracionResource();
        }

        public TranscripcionNoEncontradaException(string message, Exception inner)
            : base(message, inner)
        {
            this.configuracionResource = new ConfiguracionResource();
        }

        public override string Message
        {
            get
            {
                return configuracionResource.ObtenerConfiguracion().ObtenerMensajeTexto("TranscripcionNoEncontrada");
            }
        }

    }



}