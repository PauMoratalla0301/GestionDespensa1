using GestionDespensa1.BD.Data.Entity;

namespace GestionDespensa1.Server.Repositorio
{
    public interface IProductoRepositorio
    {
        Task<Producto?> GetByDescripcion(string descripcion);
        Task<List<Producto>> GetByCategoria(int categoriaId);
        Task<List<Producto>> GetStockBajo();
        Task<List<Producto>> SelectWithRelations();
        Task<Producto?> SelectByIdWithRelations(int id);
        Task<bool> Existe(int id);
        Task<int> Insert(Producto producto);
        Task<bool> Update(int id, Producto producto);
        Task<bool> Delete(int id);
    }
}