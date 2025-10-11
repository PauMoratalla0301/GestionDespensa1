using GestionDespensa1.Client.Servicios.Http;
using GestionDespensa1.Shared.DTO;

namespace GestionDespensa1.Client.Servicios.Entidades
{
    public interface IDetalleCompraProveedorService
    {
        Task<HttpRespuesta<List<DetalleCompraProveedorDTO>>> Get();
        Task<HttpRespuesta<DetalleCompraProveedorDTO>> Get(int id);
        Task<HttpRespuesta<List<DetalleCompraProveedorDTO>>> GetByCompra(int compraId);
        Task<HttpRespuesta<List<DetalleCompraProveedorDTO>>> GetByProducto(int productoId);
        Task<HttpRespuesta<int>> Insert(CrearDetalleCompraProveedorDTO detalleCompra);
        Task<HttpRespuesta<object>> Update(int id, DetalleCompraProveedorDTO detalleCompra);
        Task<HttpRespuesta<object>> Delete(int id);
    }
}