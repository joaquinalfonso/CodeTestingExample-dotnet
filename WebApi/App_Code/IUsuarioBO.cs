namespace WebApi.Negocio
{
    public interface IUsuarioBO
    {
        bool EsUsuarioValido(string loginUsuario);
    }
}