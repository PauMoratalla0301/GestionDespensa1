using GestionDespensa1.Client.Servicios.Http;
using GestionDespensa1.Shared.DTO;

namespace GestionDespensa1.Client.Servicios.Entidades
{
    public interface IMovimientoStockService
    {
        Task<HttpRespuesta<List<MovimientoStockDTO>>> Get();
        Task<HttpRespuesta<MovimientoStockDTO>> Get(int id);
        Task<HttpRespuesta<List<MovimientoStockDTO>>> GetByProducto(int idProducto);
        Task<HttpRespuesta<List<MovimientoStockDTO>>> GetByFecha(DateTime fecha);
        Task<HttpRespuesta<List<MovimientoStockDTO>>> GetByTipo(string tipo);
        Task<HttpRespuesta<int>> AjusteManual(CrearMovimientoStockDTO ajuste);
    }
}