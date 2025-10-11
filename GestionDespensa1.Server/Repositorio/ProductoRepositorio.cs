using Microsoft.EntityFrameworkCore;
using GestionDespensa1.BD.Data;
using GestionDespensa1.BD.Data.Entity;

namespace GestionDespensa1.Server.Repositorio
{
    public class ProductoRepositorio : Repositorio<Producto>, IProductoRepositorio
    {
        private readonly Context _context;

        public ProductoRepositorio(Context context) : base(context)
        {
            _context = context;
        }

        public async Task<Producto?> GetByDescripcion(string descripcion)
        {
            return await _context.Productos
                .Include(p => p.Categoria)
                .FirstOrDefaultAsync(p => p.Descripcion == descripcion);
        }

        public async Task<List<Producto>> GetByCategoria(int categoriaId)
        {
            return await _context.Productos
                .Include(p => p.Categoria)
                .Where(p => p.IdCategoria == categoriaId)
                .ToListAsync();
        }

        public async Task<List<Producto>> GetStockBajo()
        {
            return await _context.Productos
                .Include(p => p.Categoria)
                .Where(p => p.StockActual <= p.StockMinimo)
                .ToListAsync();
        }

        public async Task<List<Producto>> SelectWithRelations()
        {
            return await _context.Productos
                .Include(p => p.Categoria)
                .ToListAsync();
        }

        public async Task<Producto?> SelectByIdWithRelations(int id)
        {
            return await _context.Productos
                .Include(p => p.Categoria)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<bool> Existe(int id)
        {
            try
            {
                return await _context.Productos.AnyAsync(p => p.Id == id);
            }
            catch
            {
                return false;
            }
        }

        public async Task<int> Insert(Producto producto)
        {
            try
            {
                _context.Productos.Add(producto);
                await _context.SaveChangesAsync();
                return producto.Id;
            }
            catch
            {
                return -1;
            }
        }

        public async Task<bool> Update(int id, Producto producto)
        {
            try
            {
                var productoExistente = await _context.Productos.FindAsync(id);
                if (productoExistente == null)
                    return false;

                // Actualizar propiedades
                productoExistente.Descripcion = producto.Descripcion;
                productoExistente.PrecioUnitario = producto.PrecioUnitario;
                productoExistente.GananciaPorcentaje = producto.GananciaPorcentaje;
                productoExistente.StockActual = producto.StockActual;
                productoExistente.StockMinimo = producto.StockMinimo;
                productoExistente.IdCategoria = producto.IdCategoria;

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
                var producto = await _context.Productos.FindAsync(id);
                if (producto == null)
                    return false;

                _context.Productos.Remove(producto);
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