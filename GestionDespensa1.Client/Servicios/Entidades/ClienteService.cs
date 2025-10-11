using GestionDespensa1.Client.Servicios.Http;
using GestionDespensa1.Shared.DTO;
using System.Net.Http.Json;

namespace GestionDespensa1.Client.Servicios.Entidades
{
    public class ClienteService : IClienteService
    {
        private readonly IHttpServicio _httpServicio;

        public ClienteService(IHttpServicio httpServicio)
        {
            _httpServicio = httpServicio;
        }

        public async Task<HttpRespuesta<List<ClienteDTO>>> Get()
        {
            return await _httpServicio.Get<List<ClienteDTO>>("api/Clientes");
        }

        public async Task<HttpRespuesta<ClienteDTO>> Get(int id)
        {
            return await _httpServicio.Get<ClienteDTO>($"api/Clientes/{id}");
        }

        public async Task<HttpRespuesta<ClienteDTO>> GetByDni(string dni)
        {
            return await _httpServicio.Get<ClienteDTO>($"api/Clientes/GetByDni/{dni}");
        }

        public async Task<HttpRespuesta<List<ClienteDTO>>> GetConSaldoPendiente()
        {
            return await _httpServicio.Get<List<ClienteDTO>>("api/Clientes/GetConSaldoPendiente");
        }

        public async Task<HttpRespuesta<int>> Insert(CrearClienteDTO cliente)
        {
            var respuesta = await _httpServicio.Post("api/Clientes", cliente);

            if (!respuesta.Error && respuesta.HttpResponseMessage.IsSuccessStatusCode)
            {
                var id = await respuesta.HttpResponseMessage.Content.ReadFromJsonAsync<int>();
                return new HttpRespuesta<int>(id, false, respuesta.HttpResponseMessage);
            }

            return new HttpRespuesta<int>(0, true, respuesta.HttpResponseMessage);
        }

        public async Task<HttpRespuesta<object>> Update(int id, ClienteDTO cliente)
        {
            return await _httpServicio.Put($"api/Clientes/{id}", cliente);
        }

        public async Task<HttpRespuesta<object>> Delete(int id)
        {
            return await _httpServicio.Delete($"api/Clientes/{id}");
        }
    }
}
