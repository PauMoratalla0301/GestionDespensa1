using GestionDespensa1.BD.Data.Entity;
using GestionDespensa1.Shared.DTO;
using Microsoft.AspNetCore.Mvc;

namespace GestionDespensa1.Server.Repositorio
{
    public interface IClienteRepositorio
    {
        Task<Cliente?> GetByDni(string dni);
        Task<List<Cliente>> GetBySaldoPendiente();
        Task<List<Cliente>> SelectWithRelations();
        Task<Cliente?> SelectByIdWithRelations(int id);
        Task<ActionResult<bool>> Existe(int id);
        Task<ActionResult<int>> Insert(Cliente cliente);
        Task<bool> Update(int id, Cliente cliente);
        Task<bool> Delete(int id);
    }
}