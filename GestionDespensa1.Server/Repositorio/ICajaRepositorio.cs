using GestionDespensa1.BD.Data.Entity;

namespace GestionDespensa1.Server.Repositorio
{
    public interface ICajaRepositorio
    {
        Task<List<Caja>> GetByUsuario(string idUsuario);
        Task<List<Caja>> GetByFecha(DateTime fecha);
        Task<List<Caja>> SelectWithRelations();
        Task<Caja?> SelectByIdWithRelations(int id);
        Task<bool> Existe(int id);  // Cambiado de ActionResult<bool> a bool
        Task<int> Insert(Caja caja);  // Cambiado de ActionResult<int> a int
        Task<bool> Update(int id, Caja caja);
        Task<bool> Delete(int id);
    }
}