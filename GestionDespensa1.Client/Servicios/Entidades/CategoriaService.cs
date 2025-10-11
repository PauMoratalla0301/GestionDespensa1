using GestionDespensa1.Client.Servicios.Http;
using GestionDespensa1.Shared.DTO;
using System.Net.Http.Json;

namespace GestionDespensa1.Client.Servicios.Entidades
{
    public class CategoriaService : ICategoriaService
    {
        private readonly IHttpServicio _httpServicio;

        public CategoriaService(IHttpServicio httpServicio)
        {
            _httpServicio = httpServicio;
        }

        public async Task<HttpRespuesta<List<CategoriaDTO>>> Get()
        {
            return await _httpServicio.Get<List<CategoriaDTO>>("api/Categorias");
        }

        public async Task<HttpRespuesta<CategoriaDTO>> Get(int id)
        {
            return await _httpServicio.Get<CategoriaDTO>($"api/Categorias/{id}");
        }

        public async Task<HttpRespuesta<CategoriaDTO>> GetByNombre(string nombre)
        {
            return await _httpServicio.Get<CategoriaDTO>($"api/Categorias/GetByNombre/{nombre}");
        }

        public async Task<HttpRespuesta<int>> Insert(CrearCategoriaDTO categoria)
        {
            var respuesta = await _httpServicio.Post("api/Categorias", categoria);

            if (!respuesta.Error && respuesta.HttpResponseMessage.IsSuccessStatusCode)
            {
                var id = await respuesta.HttpResponseMessage.Content.ReadFromJsonAsync<int>();
                return new HttpRespuesta<int>(id, false, respuesta.HttpResponseMessage);
            }

            return new HttpRespuesta<int>(0, true, respuesta.HttpResponseMessage);
        }

        public async Task<HttpRespuesta<object>> Update(int id, CategoriaDTO categoria)
        {
            return await _httpServicio.Put($"api/Categorias/{id}", categoria);
        }

        public async Task<HttpRespuesta<object>> Delete(int id)
        {
            return await _httpServicio.Delete($"api/Categorias/{id}");
        }
    }
}
