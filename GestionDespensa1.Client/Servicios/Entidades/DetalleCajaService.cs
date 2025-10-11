using GestionDespensa1.Client.Servicios.Http;
using GestionDespensa1.Shared.DTO;
using System.Net.Http.Json;

namespace GestionDespensa1.Client.Servicios.Entidades
{
    public class DetalleCajaService : IDetalleCajaService
    {
        private readonly IHttpServicio _httpServicio;

        public DetalleCajaService(IHttpServicio httpServicio)
        {
            _httpServicio = httpServicio;
        }

        public async Task<HttpRespuesta<List<DetalleCajaDTO>>> Get()
        {
            return await _httpServicio.Get<List<DetalleCajaDTO>>("api/DetallesCaja");
        }

        public async Task<HttpRespuesta<DetalleCajaDTO>> Get(int id)
        {
            return await _httpServicio.Get<DetalleCajaDTO>($"api/DetallesCaja/{id}");
        }

        public async Task<HttpRespuesta<List<DetalleCajaDTO>>> GetByCaja(int cajaId)
        {
            return await _httpServicio.Get<List<DetalleCajaDTO>>($"api/DetallesCaja/GetByCaja/{cajaId}");
        }

        public async Task<HttpRespuesta<List<DetalleCajaDTO>>> GetByVenta(string idVenta)
        {
            return await _httpServicio.Get<List<DetalleCajaDTO>>($"api/DetallesCaja/GetByVenta/{idVenta}");
        }

        public async Task<HttpRespuesta<int>> Insert(CrearDetalleCajaDTO detalleCaja)
        {
            var respuesta = await _httpServicio.Post("api/DetallesCaja", detalleCaja);

            if (!respuesta.Error && respuesta.HttpResponseMessage.IsSuccessStatusCode)
            {
                var id = await respuesta.HttpResponseMessage.Content.ReadFromJsonAsync<int>();
                return new HttpRespuesta<int>(id, false, respuesta.HttpResponseMessage);
            }

            return new HttpRespuesta<int>(0, true, respuesta.HttpResponseMessage);
        }

        public async Task<HttpRespuesta<object>> Update(int id, DetalleCajaDTO detalleCaja)
        {
            return await _httpServicio.Put($"api/DetallesCaja/{id}", detalleCaja);
        }

        public async Task<HttpRespuesta<object>> Delete(int id)
        {
            return await _httpServicio.Delete($"api/DetallesCaja/{id}");
        }
    }
}
