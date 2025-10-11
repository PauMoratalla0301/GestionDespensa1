using GestionDespensa1.Client.Servicios.Http;
using GestionDespensa1.Shared.DTO;

namespace GestionDespensa1.Client.Servicios.Entidades
{
    public interface ICompraProveedorService
    {
        Task<HttpRespuesta<List<CompraProveedorDTO>>> Get();
        Task<HttpRespuesta<CompraProveedorDTO>> Get(int id);
        Task<HttpRespuesta<List<CompraProveedorDTO>>> GetByProveedor(int proveedorId);
        Task<HttpRespuesta<List<CompraProveedorDTO>>> GetByFecha(string fecha);
        Task<HttpRespuesta<int>> Insert(CrearCompraProveedorDTO compraProveedor);
        Task<HttpRespuesta<object>> Update(int id, CompraProveedorDTO compraProveedor);
        Task<HttpRespuesta<object>> Delete(int id);
    }
}