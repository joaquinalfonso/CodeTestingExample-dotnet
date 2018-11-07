using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApi.Comun;

namespace WebApi.Infraestructura
{


    public class ConfiguracionResource : IConfiguracionResource
    {
        public Configuracion ObtenerConfiguracion()
        {
            return new Configuracion();
        }
    }
}