using GestionDespensa1.Client.Servicios.Http;
using GestionDespensa1.Shared.DTO;
using System.Net.Http.Json;

namespace GestionDespensa1.Client.Servicios.Entidades
{
    public class CajaService : ICajaService
    {
        private readonly IHttpServicio _httpServicio;

        public CajaService(IHttpServicio httpServicio)
        {
            _httpServicio = httpServicio;
        }

        public async Task<HttpRespuesta<List<CajaDTO>>> Get()
        {
            return await _httpServicio.Get<List<CajaDTO>>("api/Cajas");
        }

        public async Task<HttpRespuesta<CajaDTO>> Get(int id)
        {
            return await _httpServicio.Get<CajaDTO>($"api/Cajas/{id}");
        }

        public async Task<HttpRespuesta<List<CajaDTO>>> GetByUsuario(string idUsuario)
        {
            return await _httpServicio.Get<List<CajaDTO>>($"api/Cajas/GetByUsuario/{idUsuario}");
        }

        public async Task<HttpRespuesta<List<CajaDTO>>> GetByFecha(DateTime fecha)
        {
            return await _httpServicio.Get<List<CajaDTO>>($"api/Cajas/GetByFecha/{fecha:yyyy-MM-dd}");
        }

        public async Task<HttpRespuesta<int>> Insert(CrearCajaDTO caja)
        {
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
    }
}
