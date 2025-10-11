using Microsoft.EntityFrameworkCore;
using GestionDespensa1.BD.Data;
using GestionDespensa1.BD.Data.Entity;

namespace GestionDespensa1.Server.Repositorio
{
    public class VentaRepositorio : Repositorio<Venta>, IVentaRepositorio
    {
        private readonly Context _context;

        public VentaRepositorio(Context context) : base(context)
        {
            _context = context;
        }

        public async Task<List<Venta>> GetByCliente(int clienteId)
        {
            return await _context.Ventas
                .Include(v => v.Cliente)
                .Include(v => v.DetallesVenta)
                    .ThenInclude(d => d.Producto)
                .Where(v => v.IdCliente == clienteId)
                .ToListAsync();
        }

        public async Task<List<Venta>> GetByFecha(DateTime fecha)
        {
            return await _context.Ventas
                .Include(v => v.Cliente)
                .Include(v => v.DetallesVenta)
                .Where(v => v.FechaVenta.Date == fecha.Date)
                .ToListAsync();
        }

        public async Task<List<Venta>> GetVentasConSaldoPendiente()
        {
            return await _context.Ventas
                .Include(v => v.Cliente)
                .Where(v => v.SaldoPendiente > 0)
                .ToListAsync();
        }

        public async Task<List<Venta>> SelectWithRelations()
        {
            return await _context.Ventas
                .Include(v => v.Cliente)
                .Include(v => v.DetallesVenta)
                    .ThenInclude(d => d.Producto)
                .ToListAsync();
        }

        public async Task<Venta?> SelectByIdWithRelations(int id)
        {
            return await _context.Ventas
                .Include(v => v.Cliente)
                .Include(v => v.DetallesVenta)
                    .ThenInclude(d => d.Producto)
                .FirstOrDefaultAsync(v => v.Id == id);
        }

        public async Task<bool> Existe(int id)
        {
            try
            {
                return await _context.Ventas.AnyAsync(v => v.Id == id);
            }
            catch
            {
                return false;
            }
        }

        public async Task<int> Insert(Venta venta)
        {
            try
            {
                _context.Ventas.Add(venta);
                await _context.SaveChangesAsync();
                return venta.Id;
            }
            catch
            {
                return -1;
            }
        }

        public async Task<bool> Update(int id, Venta venta)
        {
            try
            {
                var ventaExistente = await _context.Ventas.FindAsync(id);
                if (ventaExistente == null)
                    return false;

                // Actualizar propiedades
                ventaExistente.IdCliente = venta.IdCliente;
                ventaExistente.FechaVenta = venta.FechaVenta;
                ventaExistente.Estado = venta.Estado;
                ventaExistente.Total = venta.Total;
                ventaExistente.MontoPagado = venta.MontoPagado;
                ventaExistente.SaldoPendiente = venta.SaldoPendiente;

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
                var venta = await _context.Ventas.FindAsync(id);
                if (venta == null)
                    return false;

                _context.Ventas.Remove(venta);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        // ELIMINA ESTOS MÉTODOS DUPLICADOS:
        // Task<ActionResult<bool>> IVentaRepositorio.Existe(int id)
        // Task<ActionResult<int>> IVentaRepositorio.Insert(Venta venta)
    }
}