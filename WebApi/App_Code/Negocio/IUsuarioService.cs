namespace WebApi.Negocio
{
    public interface IUsuarioService
    {
        bool EsUsuarioValido(string loginUsuario);
    }
}