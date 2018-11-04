namespace WebApi.Servicios
{

    // Interface que define las operaciones del servicio de usuarios.

    public interface IUsuarioService
    {
        bool EsUsuarioValido(string loginUsuario);
    }
}