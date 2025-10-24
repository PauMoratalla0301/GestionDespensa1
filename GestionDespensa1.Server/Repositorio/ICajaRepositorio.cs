using GestionDespensa1.BD.Data.Entity;

namespace GestionDespensa1.Server.Repositorio
{
    public interface ICajaRepositorio
    {
        // ✅ AGREGAR este método
        Task<Caja?> GetById(int id);

        Task<List<Caja>> GetByUsuario(string idUsuario);
        Task<List<Caja>> GetByFecha(DateTime fecha);
        Task<List<Caja>> SelectWithRelations();
        Task<Caja?> SelectByIdWithRelations(int id);
        Task<bool> Existe(int id);
        Task<int> Insert(Caja caja);
        Task<bool> Update(int id, Caja caja);
        Task<bool> Delete(int id);
    }
}