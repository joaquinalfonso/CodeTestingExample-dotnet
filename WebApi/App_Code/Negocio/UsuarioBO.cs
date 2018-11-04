namespace WebApi.Negocio
{
    public class UsuarioBO : IUsuarioBO
    {

        public bool EsUsuarioValido(string loginUsuario)
        {
            return (loginUsuario != null ? loginUsuario.Length > 0 : false);
        }

    }
}