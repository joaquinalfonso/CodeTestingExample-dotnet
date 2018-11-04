namespace WebApi.Negocio
{
    public class UsuarioService : IUsuarioService
    {

        public bool EsUsuarioValido(string loginUsuario)
        {
            return (loginUsuario != null ? loginUsuario.Length > 0 : false);
        }

    }
}