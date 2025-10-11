using GestionDespensa1.BD.Data.Entity;

namespace GestionDespensa1.Server.Repositorio
{
    public interface IVentaRepositorio
    {
        Task<List<Venta>> GetByCliente(int clienteId);
        Task<List<Venta>> GetByFecha(DateTime fecha);
        Task<List<Venta>> GetVentasConSaldoPendiente();
        Task<List<Venta>> SelectWithRelations();
        Task<Venta?> SelectByIdWithRelations(int id);
        Task<bool> Existe(int id);
        Task<int> Insert(Venta venta);
        Task<bool> Update(int id, Venta venta);
        Task<bool> Delete(int id);
    }
}