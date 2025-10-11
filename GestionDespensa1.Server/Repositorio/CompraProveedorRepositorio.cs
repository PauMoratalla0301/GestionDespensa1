using Microsoft.EntityFrameworkCore;
using GestionDespensa1.BD.Data;
using GestionDespensa1.BD.Data.Entity;

namespace GestionDespensa1.Server.Repositorio
{
    public class CompraProveedorRepositorio : Repositorio<CompraProveedor>, ICompraProveedorRepositorio
    {
        private readonly Context _context;

        public CompraProveedorRepositorio(Context context) : base(context)
        {
            _context = context;
        }

        public async Task<List<CompraProveedor>> GetByProveedor(int proveedorId)
        {
            return await _context.ComprasProveedor
                .Include(c => c.Proveedor)
                .Include(c => c.DetallesCompra)
                    .ThenInclude(d => d.Producto)
                .Where(c => c.IdProveedor == proveedorId)
                .ToListAsync();
        }

        public async Task<List<CompraProveedor>> GetByFecha(string fecha)
        {
            return await _context.ComprasProveedor
                .Include(c => c.Proveedor)
                .Include(c => c.DetallesCompra)
                    .ThenInclude(d => d.Producto)
                .Where(c => c.FechaCompra == fecha)
                .ToListAsync();
        }

        public async Task<List<CompraProveedor>> SelectWithRelations()
        {
            return await _context.ComprasProveedor
                .Include(c => c.Proveedor)
                .Include(c => c.DetallesCompra)
                    .ThenInclude(d => d.Producto)
                .ToListAsync();
        }

        public async Task<CompraProveedor?> SelectByIdWithRelations(int id)
        {
            return await _context.ComprasProveedor
                .Include(c => c.Proveedor)
                .Include(c => c.DetallesCompra)
                    .ThenInclude(d => d.Producto)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<bool> Existe(int id)
        {
            try
            {
                return await _context.ComprasProveedor.AnyAsync(c => c.Id == id);
            }
            catch
            {
                return false;
            }
        }

        public async Task<int> Insert(CompraProveedor compra)
        {
            try
            {
                _context.ComprasProveedor.Add(compra);
                await _context.SaveChangesAsync();
                return compra.Id;
            }
            catch
            {
                return -1;
            }
        }

        public async Task<bool> Update(int id, CompraProveedor compra)
        {
            try
            {
                var compraExistente = await _context.ComprasProveedor.FindAsync(id);
                if (compraExistente == null)
                    return false;

                // Actualizar propiedades
                compraExistente.IdProveedor = compra.IdProveedor;
                compraExistente.FechaCompra = compra.FechaCompra;
                compraExistente.Observaciones = compra.Observaciones;

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
                var compra = await _context.ComprasProveedor.FindAsync(id);
                if (compra == null)
                    return false;

                _context.ComprasProveedor.Remove(compra);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        // ELIMINA ESTOS MÉTODOS DUPLICADOS:
        // Task<ActionResult<bool>> ICompraProveedorRepositorio.Existe(int id)
        // Task<ActionResult<int>> ICompraProveedorRepositorio.Insert(CompraProveedor compra)
    }
}