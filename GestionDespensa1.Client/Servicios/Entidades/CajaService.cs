using GestionDespensa1.Client.Servicios.Http;
using GestionDespensa1.Client.Servicios.Auth;
using GestionDespensa1.Shared.DTO;
using System.Net.Http.Json;

namespace GestionDespensa1.Client.Servicios.Entidades
{
    public class CajaService : ICajaService
    {
        private readonly IHttpServicio _httpServicio;
        private readonly IAuthService _authService;

        public CajaService(
            IHttpServicio httpServicio,
            IAuthService authService)
        {
            _httpServicio = httpServicio;
            _authService = authService;
        }

        public async Task<HttpRespuesta<List<CajaDTO>>> Get()
        {
            return await _httpServicio.Get<List<CajaDTO>>("api/Cajas");
        }

        public async Task<HttpRespuesta<CajaDTO>> Get(int id)
        {
            return await _httpServicio.Get<CajaDTO>($"api/Cajas/{id}");
        }

        public async Task<HttpRespuesta<List<CajaDTO>>> GetByUsuario(int idUsuario)
        {
            return await _httpServicio.Get<List<CajaDTO>>($"api/Cajas/GetByUsuario/{idUsuario}");
        }

        public async Task<HttpRespuesta<List<CajaDTO>>> GetByFecha(DateTime fecha)
        {
            return await _httpServicio.Get<List<CajaDTO>>($"api/Cajas/GetByFecha/{fecha:yyyy-MM-dd}");
        }

        public async Task<HttpRespuesta<int>> Insert(CrearCajaDTO caja)
        {
            // Obtener el usuario actual y asignarlo
            var usuario = await _authService.GetUsuarioActual();
            if (usuario != null)
            {
                caja.IdUsuario = usuario.Id;
            }

            var respuesta = await _httpServicio.Post("api/Cajas", caja);

            if (!respuesta.Error && respuesta.HttpResponseMessage.IsSuccessStatusCode)
            {
                var id = await respuesta.HttpResponseMessage.Content.ReadFromJsonAsync<int>();
                return new HttpRespuesta<int>(id, false, respuesta.HttpResponseMessage);
            }

            return new HttpRespuesta<int>(0, true, respuesta.HttpResponseMessage);
        }

        public async Task<HttpRespuesta<object>> Update(int id, CajaDTO caja)
        {
            return await _httpServicio.Put($"api/Cajas/{id}", caja);
        }

        public async Task<HttpRespuesta<object>> Delete(int id)
        {
            return await _httpServicio.Delete($"api/Cajas/{id}");
        }

        // 👇 NUEVO MÉTODO IMPLEMENTADO
        public async Task<HttpRespuesta<bool>> HayCajaAbiertaHoy()
        {
            return await _httpServicio.Get<bool>("api/Cajas/HayCajaAbiertaHoy");
        }
    }
}