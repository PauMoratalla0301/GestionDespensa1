// Server/Repositorio/IUsuarioRepositorio.cs
using GestionDespensa1.BD.Data.Entity;

namespace GestionDespensa1.Server.Repositorio
{
    public interface IUsuarioRepositorio : IRepositorio<Usuario>
    {
        Task<Usuario?> GetByEmail(string email);
        Task<bool> ExisteEmail(string email);
    }
}