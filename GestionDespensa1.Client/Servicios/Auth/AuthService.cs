using GestionDespensa1.Client.Servicios.Http;
using GestionDespensa1.Shared.DTO;
using Microsoft.AspNetCore.Components.Authorization;
using System.Net.Http.Headers;

namespace GestionDespensa1.Client.Servicios.Auth
{
    public class AuthService : IAuthService
    {
        private readonly IHttpServicio _httpServicio;
        private readonly CustomAuthStateProvider _authStateProvider;

        public AuthService(
            IHttpServicio httpServicio,
            AuthenticationStateProvider authStateProvider)
        {
            _httpServicio = httpServicio;
            _authStateProvider = (CustomAuthStateProvider)authStateProvider;
        }

        public async Task<HttpRespuesta<LoginRespuestaDTO>> Login(LoginDTO loginDTO)
        {
            var respuesta = await _httpServicio.Post<LoginDTO, LoginRespuestaDTO>("api/Auth/login", loginDTO);

            if (!respuesta.Error && respuesta.Respuesta?.Success == true)
            {
                await _authStateProvider.MarkUserAsAuthenticated(respuesta.Respuesta.Token!);
                _httpServicio.SetAuthorizationHeader($"Bearer {respuesta.Respuesta.Token}");

                // Recargar el usuario actual
                await GetUsuarioActual();
            }

            return respuesta;
        }

        public async Task<HttpRespuesta<LoginRespuestaDTO>> Registro(CrearUsuarioDTO crearUsuarioDTO)
        {
            return await _httpServicio.Post<CrearUsuarioDTO, LoginRespuestaDTO>("api/Auth/registro", crearUsuarioDTO);
        }

        public async Task<HttpRespuesta<LoginRespuestaDTO>> ValidarToken()
        {
            try
            {
                var token = await _authStateProvider.GetToken();

                if (string.IsNullOrEmpty(token))
                {
                    return new HttpRespuesta<LoginRespuestaDTO>(null, true, null);
                }

                _httpServicio.SetAuthorizationHeader($"Bearer {token}");
                var respuesta = await _httpServicio.Get<LoginRespuestaDTO>("api/Auth/validar-token");

                return respuesta;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error validando token: {ex.Message}");
                return new HttpRespuesta<LoginRespuestaDTO>(null, true, null);
            }
        }

        public async Task Logout()
        {
            await _authStateProvider.MarkUserAsLoggedOut();
            _httpServicio.SetAuthorizationHeader(null);
        }

        public async Task<bool> EstaAutenticado()
        {
            var token = await _authStateProvider.GetToken();
            return !string.IsNullOrEmpty(token);
        }

        public async Task<UsuarioDTO?> GetUsuarioActual()
        {
            try
            {
                var respuesta = await ValidarToken();
                if (!respuesta.Error && respuesta.Respuesta?.Success == true)
                {
                    return respuesta.Respuesta.Usuario;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error obteniendo usuario actual: {ex.Message}");
            }
            return null;
        }

        public async Task<string?> GetToken()
        {
            return await _authStateProvider.GetToken();
        }
    }
}