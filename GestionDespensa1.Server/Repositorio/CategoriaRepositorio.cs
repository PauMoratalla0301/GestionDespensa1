using Microsoft.EntityFrameworkCore;
using GestionDespensa1.BD.Data;
using GestionDespensa1.BD.Data.Entity;
using GestionDespensa1.Server.Repositorio;

namespace GestionDespensa1.Server.Repositorio
{
    public class CategoriaRepositorio : Repositorio<Categoria>, ICategoriaRepositorio
    {
        private readonly Context _context;

        public CategoriaRepositorio(Context context) : base(context)
        {
            _context = context;
        }

        public async Task<Categoria?> GetByNombre(string nombre)
        {
            return await _context.Categorias
                .Include(c => c.Productos)
                .FirstOrDefaultAsync(c => c.NombreCategoria == nombre);
        }

        public async Task<List<Categoria>> SelectWithRelations()
        {
            return await _context.Categorias
                .Include(c => c.Productos)
                .ToListAsync();
        }

        public async Task<Categoria?> SelectByIdWithRelations(int id)
        {
            return await _context.Categorias
                .Include(c => c.Productos)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<bool> Existe(int id)
        {
            try
            {
                return await _context.Categorias.AnyAsync(c => c.Id == id);
            }
            catch
            {
                return false;
            }
        }

        public async Task<int> Insert(Categoria categoria)
        {
            try
            {
                _context.Categorias.Add(categoria);
                await _context.SaveChangesAsync();
                return categoria.Id;
            }
            catch
            {
                return -1;
            }
        }

        public async Task<bool> Update(int id, Categoria categoria)
        {
            try
            {
                var categoriaExistente = await _context.Categorias.FindAsync(id);
                if (categoriaExistente == null)
                    return false;

                // SOLO ACTUALIZAR NombreCategoria (no existe Descripcion)
                categoriaExistente.NombreCategoria = categoria.NombreCategoria;

                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> Delete(int id)
        {
            try
            {
                var categoria = await _context.Categorias.FindAsync(id);
                if (categoria == null)
                    return false;

                _context.Categorias.Remove(categoria);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}