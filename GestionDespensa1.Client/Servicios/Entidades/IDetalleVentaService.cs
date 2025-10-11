using GestionDespensa1.Client.Servicios.Http;
using GestionDespensa1.Shared.DTO;

namespace GestionDespensa1.Client.Servicios.Entidades
{
    public interface IDetalleVentaService
    {
        Task<HttpRespuesta<List<DetalleVentaDTO>>> Get();
        Task<HttpRespuesta<DetalleVentaDTO>> Get(int id);
        Task<HttpRespuesta<List<DetalleVentaDTO>>> GetByVenta(int ventaId);
        Task<HttpRespuesta<List<DetalleVentaDTO>>> GetByProducto(int productoId);
        Task<HttpRespuesta<int>> Insert(CrearDetalleVentaDTO detalleVenta);
        Task<HttpRespuesta<object>> Update(int id, DetalleVentaDTO detalleVenta);
        Task<HttpRespuesta<object>> Delete(int id);
    }
}