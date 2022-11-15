using Cliente.Shared;

namespace Cliente.Client.Auth
{
	public interface IAuth
	{
        Task Login(string token);
        Task Logout();
        Task<UsuarioDTO?> GetUsuario();
    }
}
