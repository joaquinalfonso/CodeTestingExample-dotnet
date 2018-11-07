using System;
using WebApi.Comun;
using WebApi.Infraestructura;

namespace WebApi.Servicios
{

    // Excepción para notificar que ha ocurrido un error al guardar la transcripción.

    public class TranscripcionNoGuardadaException : Exception
    {

        public IConfiguracionResource configuracionResource { private get; set; }


        public TranscripcionNoGuardadaException()
        {
            this.configuracionResource = new ConfiguracionResource();
        }

        public TranscripcionNoGuardadaException(string message)
            : base(message)
        {
            this.configuracionResource = new ConfiguracionResource();
        }

        public TranscripcionNoGuardadaException(string message, Exception inner)
            : base(message, inner)
        {
            this.configuracionResource = new ConfiguracionResource();
        }

        public override string Message
        {
            get
            {
                return configuracionResource.ObtenerConfiguracion().ObtenerMensajeTexto("TranscripcionNoGuardada");
            }
        }

    }



}