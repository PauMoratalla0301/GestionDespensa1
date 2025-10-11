using GestionDespensa1.Client.Servicios.Http;
using GestionDespensa1.Shared.DTO;
using System.Net.Http.Json;

namespace GestionDespensa1.Client.Servicios.Entidades
{
    public class ProveedorService : IProveedorService
    {
        private readonly IHttpServicio _httpServicio;

        public ProveedorService(IHttpServicio httpServicio)
        {
            _httpServicio = httpServicio;
        }

        public async Task<HttpRespuesta<List<ProveedorDTO>>> Get()
        {
            return await _httpServicio.Get<List<ProveedorDTO>>("api/Proveedores");
        }

        public async Task<HttpRespuesta<ProveedorDTO>> Get(int id)
        {
            return await _httpServicio.Get<ProveedorDTO>($"api/Proveedores/{id}");
        }

        public async Task<HttpRespuesta<ProveedorDTO>> GetByCuit(string cuit)
        {
            return await _httpServicio.Get<ProveedorDTO>($"api/Proveedores/GetByCuit/{cuit}");
        }

        public async Task<HttpRespuesta<List<ProveedorDTO>>> GetByEstado(string estado)
        {
            return await _httpServicio.Get<List<ProveedorDTO>>($"api/Proveedores/GetByEstado/{estado}");
        }

        public async Task<HttpRespuesta<int>> Insert(CrearProveedorDTO proveedor)
        {
            var respuesta = await _httpServicio.Post("api/Proveedores", proveedor);

            if (!respuesta.Error && respuesta.HttpResponseMessage.IsSuccessStatusCode)
            {
                var id = await respuesta.HttpResponseMessage.Content.ReadFromJsonAsync<int>();
                return new HttpRespuesta<int>(id, false, respuesta.HttpResponseMessage);
            }

            return new HttpRespuesta<int>(0, true, respuesta.HttpResponseMessage);
        }

        public async Task<HttpRespuesta<object>> Update(int id, ProveedorDTO proveedor)
        {
            return await _httpServicio.Put($"api/Proveedores/{id}", proveedor);
        }

        public async Task<HttpRespuesta<object>> Delete(int id)
        {
            return await _httpServicio.Delete($"api/Proveedores/{id}");
        }
    }
}
