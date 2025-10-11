using GestionDespensa1.BD.Data.Entity;

namespace GestionDespensa1.Server.Repositorio
{
    public interface ICategoriaRepositorio
    {
        Task<Categoria?> GetByNombre(string nombre);
        Task<List<Categoria>> SelectWithRelations();
        Task<Categoria?> SelectByIdWithRelations(int id);
        Task<bool> Existe(int id);
        Task<int> Insert(Categoria categoria);
        Task<bool> Update(int id, Categoria categoria);
        Task<bool> Delete(int id);
    }
}