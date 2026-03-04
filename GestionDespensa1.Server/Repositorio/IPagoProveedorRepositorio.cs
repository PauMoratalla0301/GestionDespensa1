using GestionDespensa1.BD.Data.Entity;

namespace GestionDespensa1.Server.Repositorio
{
    public interface IPagoProveedorRepositorio : IRepositorio<PagoProveedor>
    {
        Task<List<PagoProveedor>> GetByCompra(int idCompra);
        Task<List<PagoProveedor>> GetByProveedor(int idProveedor);
        Task<decimal> GetTotalPagadoPorCompra(int idCompra);
    }
}