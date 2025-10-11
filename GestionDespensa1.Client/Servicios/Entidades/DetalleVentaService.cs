using GestionDespensa1.Client.Servicios.Http;
using GestionDespensa1.Shared.DTO;
using System.Net.Http.Json;

namespace GestionDespensa1.Client.Servicios.Entidades
{
    public class DetalleVentaService : IDetalleVentaService
    {
        private readonly IHttpServicio _httpServicio;

        public DetalleVentaService(IHttpServicio httpServicio)
        {
            _httpServicio = httpServicio;
        }

        public async Task<HttpRespuesta<List<DetalleVentaDTO>>> Get()
        {
            return await _httpServicio.Get<List<DetalleVentaDTO>>("api/DetallesVenta");
        }

        public async Task<HttpRespuesta<DetalleVentaDTO>> Get(int id)
        {
            return await _httpServicio.Get<DetalleVentaDTO>($"api/DetallesVenta/{id}");
        }

        public async Task<HttpRespuesta<List<DetalleVentaDTO>>> GetByVenta(int ventaId)
        {
            return await _httpServicio.Get<List<DetalleVentaDTO>>($"api/DetallesVenta/GetByVenta/{ventaId}");
        }

        public async Task<HttpRespuesta<List<DetalleVentaDTO>>> GetByProducto(int productoId)
        {
            return await _httpServicio.Get<List<DetalleVentaDTO>>($"api/DetallesVenta/GetByProducto/{productoId}");
        }

        public async Task<HttpRespuesta<int>> Insert(CrearDetalleVentaDTO detalleVenta)
        {
            var respuesta = await _httpServicio.Post("api/DetallesVenta", detalleVenta);

            if (!respuesta.Error && respuesta.HttpResponseMessage.IsSuccessStatusCode)
            {
                var id = await respuesta.HttpResponseMessage.Content.ReadFromJsonAsync<int>();
                return new HttpRespuesta<int>(id, false, respuesta.HttpResponseMessage);
            }

            return new HttpRespuesta<int>(0, true, respuesta.HttpResponseMessage);
        }

        public async Task<HttpRespuesta<object>> Update(int id, DetalleVentaDTO detalleVenta)
        {
            return await _httpServicio.Put($"api/DetallesVenta/{id}", detalleVenta);
        }

        public async Task<HttpRespuesta<object>> Delete(int id)
        {
            return await _httpServicio.Delete($"api/DetallesVenta/{id}");
        }
    }
}
