using GestionDespensa1.Client.Servicios.Http;
using GestionDespensa1.Shared.DTO;
using System.Net.Http.Json;

namespace GestionDespensa1.Client.Servicios.Entidades
{
    public class CompraProveedorService : ICompraProveedorService
    {
        private readonly IHttpServicio _httpServicio;

        public CompraProveedorService(IHttpServicio httpServicio)
        {
            _httpServicio = httpServicio;
        }

        public async Task<HttpRespuesta<List<CompraProveedorDTO>>> Get()
        {
            return await _httpServicio.Get<List<CompraProveedorDTO>>("api/ComprasProveedor");
        }

        public async Task<HttpRespuesta<CompraProveedorDTO>> Get(int id)
        {
            return await _httpServicio.Get<CompraProveedorDTO>($"api/ComprasProveedor/{id}");
        }

        public async Task<HttpRespuesta<List<CompraProveedorDTO>>> GetByProveedor(int proveedorId)
        {
            return await _httpServicio.Get<List<CompraProveedorDTO>>($"api/ComprasProveedor/GetByProveedor/{proveedorId}");
        }

        public async Task<HttpRespuesta<List<CompraProveedorDTO>>> GetByFecha(string fecha)
        {
            return await _httpServicio.Get<List<CompraProveedorDTO>>($"api/ComprasProveedor/GetByFecha/{fecha}");
        }

        public async Task<HttpRespuesta<int>> Insert(CrearCompraProveedorDTO compraProveedor)
        {
            var respuesta = await _httpServicio.Post("api/ComprasProveedor", compraProveedor);

            if (!respuesta.Error && respuesta.HttpResponseMessage.IsSuccessStatusCode)
            {
                var id = await respuesta.HttpResponseMessage.Content.ReadFromJsonAsync<int>();
                return new HttpRespuesta<int>(id, false, respuesta.HttpResponseMessage);
            }

            return new HttpRespuesta<int>(0, true, respuesta.HttpResponseMessage);
        }

        public async Task<HttpRespuesta<object>> Update(int id, CompraProveedorDTO compraProveedor)
        {
            return await _httpServicio.Put($"api/ComprasProveedor/{id}", compraProveedor);
        }

        public async Task<HttpRespuesta<object>> Delete(int id)
        {
            return await _httpServicio.Delete($"api/ComprasProveedor/{id}");
        }
    }
}
