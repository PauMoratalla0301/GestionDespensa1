using GestionDespensa1.Client.Servicios.Http;
using GestionDespensa1.Shared.DTO;

namespace GestionDespensa1.Client.Servicios.Auth
{
    public interface IAuthService
    {
        Task<HttpRespuesta<LoginRespuestaDTO>> Login(LoginDTO loginDTO);
        Task<HttpRespuesta<LoginRespuestaDTO>> Registro(CrearUsuarioDTO crearUsuarioDTO);
        Task<HttpRespuesta<LoginRespuestaDTO>> ValidarToken();
        Task Logout();
        Task<bool> EstaAutenticado();
        Task<UsuarioDTO?> GetUsuarioActual();
        Task<string?> GetToken();
    }
}