using GestionDespensa1.Client.Servicios.Http;
using GestionDespensa1.Shared.DTO;

namespace GestionDespensa1.Client.Servicios.Entidades
{
    public class MovimientoStockService : IMovimientoStockService
    {
        private readonly IHttpServicio _httpServicio;

        public MovimientoStockService(IHttpServicio httpServicio)
        {
            _httpServicio = httpServicio;
        }

        public async Task<HttpRespuesta<List<MovimientoStockDTO>>> Get()
        {
            return await _httpServicio.Get<List<MovimientoStockDTO>>("api/MovimientoStock");
        }

        public async Task<HttpRespuesta<MovimientoStockDTO>> Get(int id)
        {
            return await _httpServicio.Get<MovimientoStockDTO>($"api/MovimientoStock/{id}");
        }

        public async Task<HttpRespuesta<List<MovimientoStockDTO>>> GetByProducto(int idProducto)
        {
            return await _httpServicio.Get<List<MovimientoStockDTO>>($"api/MovimientoStock/GetByProducto/{idProducto}");
        }

        public async Task<HttpRespuesta<List<MovimientoStockDTO>>> GetByFecha(DateTime fecha)
        {
            return await _httpServicio.Get<List<MovimientoStockDTO>>($"api/MovimientoStock/GetByFecha/{fecha:yyyy-MM-dd}");
        }

        public async Task<HttpRespuesta<List<MovimientoStockDTO>>> GetByTipo(string tipo)
        {
            return await _httpServicio.Get<List<MovimientoStockDTO>>($"api/MovimientoStock/GetByTipo/{tipo}");
        }

        public async Task<HttpRespuesta<int>> AjusteManual(CrearMovimientoStockDTO ajuste)
        {
            return await _httpServicio.Post<CrearMovimientoStockDTO, int>("api/MovimientoStock/ajuste", ajuste);
        }
    }
}