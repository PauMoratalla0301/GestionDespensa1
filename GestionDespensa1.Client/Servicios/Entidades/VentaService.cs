using GestionDespensa1.Client.Servicios.Auth;
using GestionDespensa1.Client.Servicios.Http;
using GestionDespensa1.Shared.DTO;
using System.Net.Http.Json;

namespace GestionDespensa1.Client.Servicios.Entidades
{
    public class VentaService : IVentaService
    {
        private readonly IHttpServicio _httpServicio;
        private readonly IAuthService _authService;  // 👈 NUEVO

        public VentaService(
            IHttpServicio httpServicio,
            IAuthService authService)  // 👈 NUEVO
        {
            _httpServicio = httpServicio;
            _authService = authService;
        }

        public async Task<HttpRespuesta<List<VentaDTO>>> Get()
        {
            return await _httpServicio.Get<List<VentaDTO>>("api/Ventas");
        }

        public async Task<HttpRespuesta<VentaDTO>> Get(int id)
        {
            return await _httpServicio.Get<VentaDTO>($"api/Ventas/{id}");
        }

        public async Task<HttpRespuesta<List<VentaDTO>>> GetConSaldoPendiente()
        {
            return await _httpServicio.Get<List<VentaDTO>>("api/Ventas/GetConSaldoPendiente");
        }

        public async Task<HttpRespuesta<int>> Insert(CrearVentaDTO venta)
        {
            // 👇 OBTENER USUARIO ACTUAL
            var usuario = await _authService.GetUsuarioActual();
            if (usuario != null)
            {
                venta.IdUsuario = usuario.Id;
            }

            return await _httpServicio.Post<CrearVentaDTO, int>("api/Ventas", venta);
        }

        public async Task<HttpRespuesta<object>> Update(int id, VentaDTO venta)
        {
            return await _httpServicio.Put($"api/Ventas/{id}", venta);
        }

        public async Task<HttpRespuesta<object>> Delete(int id)
        {
            return await _httpServicio.Delete($"api/Ventas/{id}");
        }

        public async Task<HttpRespuesta<ResumenVentasDTO>> GetResumenPorFecha(DateTime fecha)
        {
            return await _httpServicio.Get<ResumenVentasDTO>($"api/Ventas/GetResumenPorFecha/{fecha:yyyy-MM-dd}");
        }

        public async Task<HttpRespuesta<bool>> SaldarVentaSimple(int id)
        {
            return await _httpServicio.Post<object, bool>($"api/Ventas/SaldarVentaSimple/{id}", new { });
        }

        public Task<HttpRespuesta<List<VentaDTO>>> GetByCliente(int clienteId)
        {
            throw new NotImplementedException();
        }

        public Task<HttpRespuesta<List<VentaDTO>>> GetByFecha(DateTime fecha)
        {
            throw new NotImplementedException();
        }
    }
}