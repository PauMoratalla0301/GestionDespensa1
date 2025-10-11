using GestionDespensa1.BD.Data.Entity;

namespace GestionDespensa1.Server.Repositorio
{
    public interface IDetalleCompraProveedorRepositorio
    {
        Task<List<DetalleCompraProveedor>> GetByCompra(int compraId);
        Task<List<DetalleCompraProveedor>> GetByProducto(int productoId);
        Task<List<DetalleCompraProveedor>> SelectWithRelations();
        Task<DetalleCompraProveedor?> SelectByIdWithRelations(int id);
        Task<bool> Existe(int id);
        Task<int> Insert(DetalleCompraProveedor detalle);
        Task<bool> Update(int id, DetalleCompraProveedor detalle);
        Task<bool> Delete(int id);
    }
}