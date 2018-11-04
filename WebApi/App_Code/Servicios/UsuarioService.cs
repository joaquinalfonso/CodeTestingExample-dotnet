namespace WebApi.Servicios
{

    // Clase de servicios que implementa la logica de negocio de las operaciones de usuarios.
    // Implementa la interface IUsuarioService

    public class UsuarioService : IUsuarioService
    {

        public bool EsUsuarioValido(string loginUsuario)
        {
            return (loginUsuario != null ? loginUsuario.Length > 0 : false);
        }

    }
}