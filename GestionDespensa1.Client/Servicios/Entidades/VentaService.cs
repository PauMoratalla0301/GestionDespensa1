using GestionDespensa1.Client.Servicios.Http;
using GestionDespensa1.Shared.DTO;
using System.Net.Http.Json;

namespace GestionDespensa1.Client.Servicios.Entidades
{
    public class VentaService : IVentaService
    {
        private readonly IHttpServicio _httpServicio;

        public VentaService(IHttpServicio httpServicio)
        {
            _httpServicio = httpServicio;
        }

        public async Task<HttpRespuesta<List<VentaDTO>>> Get()
        {
            return await _httpServicio.Get<List<VentaDTO>>("api/Ventas");
        }

        public async Task<HttpRespuesta<VentaDTO>> Get(int id)
        {
            return await _httpServicio.Get<VentaDTO>($"api/Ventas/{id}");
        }

        public async Task<HttpRespuesta<List<VentaDTO>>> GetByCliente(int clienteId)
        {
            return await _httpServicio.Get<List<VentaDTO>>($"api/Ventas/GetByCliente/{clienteId}");
        }

        public async Task<HttpRespuesta<List<VentaDTO>>> GetByFecha(DateTime fecha)
        {
            return await _httpServicio.Get<List<VentaDTO>>($"api/Ventas/GetByFecha/{fecha:yyyy-MM-dd}");
        }

        public async Task<HttpRespuesta<List<VentaDTO>>> GetConSaldoPendiente()
        {
            return await _httpServicio.Get<List<VentaDTO>>("api/Ventas/GetConSaldoPendiente");
        }

        public async Task<HttpRespuesta<int>> Insert(CrearVentaDTO venta)
        {
            var respuesta = await _httpServicio.Post("api/Ventas", venta);

            if (!respuesta.Error && respuesta.HttpResponseMessage.IsSuccessStatusCode)
            {
                var id = await respuesta.HttpResponseMessage.Content.ReadFromJsonAsync<int>();
                return new HttpRespuesta<int>(id, false, respuesta.HttpResponseMessage);
            }

            return new HttpRespuesta<int>(0, true, respuesta.HttpResponseMessage);
        }

        public async Task<HttpRespuesta<object>> Update(int id, VentaDTO venta)
        {
            return await _httpServicio.Put($"api/Ventas/{id}", venta);
        }

        public async Task<HttpRespuesta<object>> Delete(int id)
        {
            return await _httpServicio.Delete($"api/Ventas/{id}");
        }

        public Task<HttpRespuesta<ResumenVentasDTO>> GetResumenPorFecha(DateTime fecha)
        {
            throw new NotImplementedException();
        }
    }
}
