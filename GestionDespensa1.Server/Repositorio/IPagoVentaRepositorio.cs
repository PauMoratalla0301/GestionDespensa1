using GestionDespensa1.BD.Data.Entity;

namespace GestionDespensa1.Server.Repositorio
{
    public interface IPagoVentaRepositorio : IRepositorio<PagoVenta>
    {
        Task<List<PagoVenta>> GetByVenta(int idVenta);
        Task<List<PagoVenta>> GetByCliente(int idCliente);
        Task<decimal> GetTotalPagadoPorVenta(int idVenta);
    }
}