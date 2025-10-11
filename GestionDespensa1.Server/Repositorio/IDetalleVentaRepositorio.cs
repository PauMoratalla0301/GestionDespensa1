using GestionDespensa1.BD.Data.Entity;

namespace GestionDespensa1.Server.Repositorio
{
    public interface IDetalleVentaRepositorio
    {
        Task<List<DetalleVenta>> GetByVenta(int ventaId);
        Task<List<DetalleVenta>> GetByProducto(int productoId);
        Task<List<DetalleVenta>> SelectWithRelations();
        Task<DetalleVenta?> SelectByIdWithRelations(int id);
        Task<bool> Existe(int id);
        Task<int> Insert(DetalleVenta detalle);
        Task<bool> Update(int id, DetalleVenta detalle);
        Task<bool> Delete(int id);
    }
}