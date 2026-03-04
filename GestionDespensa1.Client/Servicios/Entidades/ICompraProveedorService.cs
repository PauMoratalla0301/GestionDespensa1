using GestionDespensa1.Client.Servicios.Http;
using GestionDespensa1.Shared.DTO;

namespace GestionDespensa1.Client.Servicios.Entidades
{
    public interface ICompraProveedorService
    {
        Task<HttpRespuesta<List<CompraProveedorDTO>>> Get();
        Task<HttpRespuesta<CompraProveedorDTO>> Get(int id);
        Task<HttpRespuesta<List<CompraProveedorDTO>>> GetByProveedor(int idProveedor);
        Task<HttpRespuesta<int>> Insert(CrearCompraProveedorDTO compra);
        Task<HttpRespuesta<object>> Update(int id, CompraProveedorDTO compra);
        Task<HttpRespuesta<object>> Delete(int id);
    }
}