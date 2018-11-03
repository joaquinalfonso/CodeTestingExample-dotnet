namespace WebApi.App_Code
{
    public class UsuarioBR
    {

        public bool EsUsuarioValido(string loginUsuario)
        {
            return (loginUsuario != null ? loginUsuario.Length > 0 : false);
        }

    }
}