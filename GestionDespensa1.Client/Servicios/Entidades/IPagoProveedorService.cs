using GestionDespensa1.Client.Servicios.Http;
using GestionDespensa1.Shared.DTO;

namespace GestionDespensa1.Client.Servicios.Entidades
{
    public interface IPagoProveedorService
    {
        Task<HttpRespuesta<List<PagoProveedorDTO>>> GetByCompra(int idCompra);
        Task<HttpRespuesta<List<PagoProveedorDTO>>> GetByProveedor(int idProveedor);
        Task<HttpRespuesta<int>> Insert(CrearPagoProveedorDTO pago);
    }
}