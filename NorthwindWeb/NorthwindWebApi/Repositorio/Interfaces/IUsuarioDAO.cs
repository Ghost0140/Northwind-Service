using NorthwindWebApi.Models;

namespace NorthwindWebApi.Repositorio.Interfaces
{
    public interface IUsuarioDAO
    {
        LoginResponse ValidarLogin(LoginRequest request);
        List<Usuario> ListarUsuarios();
        string RegistrarUsuario(Usuario usuario);
        string EliminarUsuario(int id);
        Usuario ObtenerUsuario(int id);
        string ActualizarUsuario(Usuario usuario);
    }
}