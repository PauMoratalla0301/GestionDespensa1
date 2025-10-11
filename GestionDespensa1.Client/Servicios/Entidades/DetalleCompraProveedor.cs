using GestionDespensa1.Client.Servicios.Http;
using GestionDespensa1.Shared.DTO;
using System.Net.Http.Json;

namespace GestionDespensa1.Client.Servicios.Entidades
{
    public class DetalleCompraProveedorService : IDetalleCompraProveedorService
    {
        private readonly IHttpServicio _httpServicio;

        public DetalleCompraProveedorService(IHttpServicio httpServicio)
        {
            _httpServicio = httpServicio;
        }

        public async Task<HttpRespuesta<List<DetalleCompraProveedorDTO>>> Get()
        {
            return await _httpServicio.Get<List<DetalleCompraProveedorDTO>>("api/DetallesCompraProveedor");
        }

        public async Task<HttpRespuesta<DetalleCompraProveedorDTO>> Get(int id)
        {
            return await _httpServicio.Get<DetalleCompraProveedorDTO>($"api/DetallesCompraProveedor/{id}");
        }

        public async Task<HttpRespuesta<List<DetalleCompraProveedorDTO>>> GetByCompra(int compraId)
        {
            return await _httpServicio.Get<List<DetalleCompraProveedorDTO>>($"api/DetallesCompraProveedor/GetByCompra/{compraId}");
        }

        public async Task<HttpRespuesta<List<DetalleCompraProveedorDTO>>> GetByProducto(int productoId)
        {
            return await _httpServicio.Get<List<DetalleCompraProveedorDTO>>($"api/DetallesCompraProveedor/GetByProducto/{productoId}");
        }

        public async Task<HttpRespuesta<int>> Insert(CrearDetalleCompraProveedorDTO detalleCompra)
        {
            var respuesta = await _httpServicio.Post("api/DetallesCompraProveedor", detalleCompra);

            if (!respuesta.Error && respuesta.HttpResponseMessage.IsSuccessStatusCode)
            {
                var id = await respuesta.HttpResponseMessage.Content.ReadFromJsonAsync<int>();
                return new HttpRespuesta<int>(id, false, respuesta.HttpResponseMessage);
            }

            return new HttpRespuesta<int>(0, true, respuesta.HttpResponseMessage);
        }

        public async Task<HttpRespuesta<object>> Update(int id, DetalleCompraProveedorDTO detalleCompra)
        {
            return await _httpServicio.Put($"api/DetallesCompraProveedor/{id}", detalleCompra);
        }

        public async Task<HttpRespuesta<object>> Delete(int id)
        {
            return await _httpServicio.Delete($"api/DetallesCompraProveedor/{id}");
        }
    }
}
