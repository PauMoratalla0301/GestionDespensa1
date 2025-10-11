using Microsoft.EntityFrameworkCore;
using GestionDespensa1.BD.Data;
using GestionDespensa1.BD.Data.Entity;

namespace GestionDespensa1.Server.Repositorio
{
    public class DetalleVentaRepositorio : Repositorio<DetalleVenta>, IDetalleVentaRepositorio
    {
        private readonly Context _context;

        public DetalleVentaRepositorio(Context context) : base(context)
        {
            _context = context;
        }

        public async Task<List<DetalleVenta>> GetByVenta(int ventaId)
        {
            return await _context.DetallesVenta
                .Include(d => d.Venta)
                .Include(d => d.Producto)
                .Where(d => d.IdVenta == ventaId)
                .ToListAsync();
        }

        public async Task<List<DetalleVenta>> GetByProducto(int productoId)
        {
            return await _context.DetallesVenta
                .Include(d => d.Venta)
                .Include(d => d.Producto)
                .Where(d => d.IdProducto == productoId)
                .ToListAsync();
        }

        public async Task<List<DetalleVenta>> SelectWithRelations()
        {
            return await _context.DetallesVenta
                .Include(d => d.Venta)
                .Include(d => d.Producto)
                .ToListAsync();
        }

        public async Task<DetalleVenta?> SelectByIdWithRelations(int id)
        {
            return await _context.DetallesVenta
                .Include(d => d.Venta)
                .Include(d => d.Producto)
                .FirstOrDefaultAsync(d => d.Id == id);
        }

        public async Task<bool> Existe(int id)
        {
            try
            {
                return await _context.DetallesVenta.AnyAsync(d => d.Id == id);
            }
            catch
            {
                return false;
            }
        }

        public async Task<int> Insert(DetalleVenta detalle)
        {
            try
            {
                _context.DetallesVenta.Add(detalle);
                await _context.SaveChangesAsync();
                return detalle.Id;
            }
            catch
            {
                return -1;
            }
        }

        public async Task<bool> Update(int id, DetalleVenta detalle)
        {
            try
            {
                var detalleExistente = await _context.DetallesVenta.FindAsync(id);
                if (detalleExistente == null)
                    return false;

                // Actualizar propiedades
                detalleExistente.IdVenta = detalle.IdVenta;
                detalleExistente.IdProducto = detalle.IdProducto;
                detalleExistente.Cantidad = detalle.Cantidad;
                detalleExistente.PrecioUnitario = detalle.PrecioUnitario;

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
                var detalle = await _context.DetallesVenta.FindAsync(id);
                if (detalle == null)
                    return false;

                _context.DetallesVenta.Remove(detalle);
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