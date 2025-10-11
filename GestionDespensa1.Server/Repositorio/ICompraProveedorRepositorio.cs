using GestionDespensa1.BD.Data.Entity;

namespace GestionDespensa1.Server.Repositorio
{
    public interface ICompraProveedorRepositorio
    {
        Task<List<CompraProveedor>> GetByProveedor(int proveedorId);
        Task<List<CompraProveedor>> GetByFecha(string fecha);
        Task<List<CompraProveedor>> SelectWithRelations();
        Task<CompraProveedor?> SelectByIdWithRelations(int id);
        Task<bool> Existe(int id);
        Task<int> Insert(CompraProveedor compra);
        Task<bool> Update(int id, CompraProveedor compra);
        Task<bool> Delete(int id);
    }
}