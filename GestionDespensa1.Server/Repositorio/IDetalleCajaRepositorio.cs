using GestionDespensa1.BD.Data.Entity;

namespace GestionDespensa1.Server.Repositorio
{
    public interface IDetalleCajaRepositorio
    {
        Task<List<DetalleCaja>> GetByCaja(int cajaId);
        Task<List<DetalleCaja>> GetByVenta(string idVenta);
        Task<List<DetalleCaja>> SelectWithRelations();
        Task<DetalleCaja?> SelectByIdWithRelations(int id);
        Task<bool> Existe(int id);
        Task<int> Insert(DetalleCaja detalle);
        Task<bool> Update(int id, DetalleCaja detalle);
        Task<bool> Delete(int id);
    }
}