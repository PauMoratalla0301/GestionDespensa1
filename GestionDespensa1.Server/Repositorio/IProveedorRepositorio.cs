using GestionDespensa1.BD.Data.Entity;

namespace GestionDespensa1.Server.Repositorio
{
    public interface IProveedorRepositorio
    {
        Task<Proveedor?> GetByCuit(string cuit);
        Task<List<Proveedor>> GetByEstado(string estado);
        Task<List<Proveedor>> SelectWithRelations();
        Task<Proveedor?> SelectByIdWithRelations(int id);
        Task<bool> Existe(int id);
        Task<int> Insert(Proveedor proveedor);
        Task<bool> Update(int id, Proveedor proveedor);
        Task<bool> Delete(int id);
    }
}