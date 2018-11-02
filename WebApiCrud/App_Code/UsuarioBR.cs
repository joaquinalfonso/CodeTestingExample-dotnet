using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApiCrud.App_Code
{
    public class UsuarioBR
    {

        public bool EsUsuarioValido(string loginUsuario)
        {
            return (loginUsuario != null ? loginUsuario.Length > 0 : false);
        }

    }
}