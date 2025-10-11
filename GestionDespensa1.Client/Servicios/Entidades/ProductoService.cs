using GestionDespensa1.Client.Servicios.Http;
using GestionDespensa1.Shared.DTO;
using System.Net.Http.Json;

namespace GestionDespensa1.Client.Servicios.Entidades
{
    public class ProductoService : IProductoService
    {
        private readonly IHttpServicio _httpServicio;

        public ProductoService(IHttpServicio httpServicio)
        {
            _httpServicio = httpServicio;
        }

        public async Task<HttpRespuesta<List<ProductoDTO>>> Get()
        {
            return await _httpServicio.Get<List<ProductoDTO>>("api/Productos");
        }

        public async Task<HttpRespuesta<ProductoDTO>> Get(int id)
        {
            return await _httpServicio.Get<ProductoDTO>($"api/Productos/{id}");
        }

        public async Task<HttpRespuesta<ProductoDTO>> GetByDescripcion(string descripcion)
        {
            return await _httpServicio.Get<ProductoDTO>($"api/Productos/GetByDescripcion/{descripcion}");
        }

        public async Task<HttpRespuesta<List<ProductoDTO>>> GetByCategoria(int categoriaId)
        {
            return await _httpServicio.Get<List<ProductoDTO>>($"api/Productos/GetByCategoria/{categoriaId}");
        }

        public async Task<HttpRespuesta<List<ProductoDTO>>> GetStockBajo()
        {
            return await _httpServicio.Get<List<ProductoDTO>>("api/Productos/GetStockBajo");
        }

        public async Task<HttpRespuesta<int>> Insert(CrearProductoDTO producto)
        {
            var respuesta = await _httpServicio.Post("api/Productos", producto);

            if (!respuesta.Error && respuesta.HttpResponseMessage.IsSuccessStatusCode)
            {
                var id = await respuesta.HttpResponseMessage.Content.ReadFromJsonAsync<int>();
                return new HttpRespuesta<int>(id, false, respuesta.HttpResponseMessage);
            }

            return new HttpRespuesta<int>(0, true, respuesta.HttpResponseMessage);
        }

        public async Task<HttpRespuesta<object>> Update(int id, ProductoDTO producto)
        {
            return await _httpServicio.Put($"api/Productos/{id}", producto);
        }

        public async Task<HttpRespuesta<object>> Delete(int id)
        {
            return await _httpServicio.Delete($"api/Productos/{id}");
        }
    }
}
