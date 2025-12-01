using Microsoft.EntityFrameworkCore;
using GestionDespensa1.BD.Data;
using GestionDespensa1.BD.Data.Entity;

namespace GestionDespensa1.Server.Repositorio
{
    public class ProveedorRepositorio : Repositorio<Proveedor>, IProveedorRepositorio
    {
        private readonly Context _context;

        public ProveedorRepositorio(Context context) : base(context)
        {
            _context = context;
        }

        public async Task<Proveedor?> GetByCuit(string cuit)
        {
            return await _context.Proveedores
                .Include(p => p.ComprasProveedor)
                    .ThenInclude(c => c.DetallesCompra)
                        .ThenInclude(d => d.Producto)
                .FirstOrDefaultAsync(p => p.CUIT == cuit);
        }

        public async Task<List<Proveedor>> GetByEstado(string estado)
        {
            var proveedores = await _context.Proveedores
                .Include(p => p.ComprasProveedor)
                    .ThenInclude(c => c.DetallesCompra)
                        .ThenInclude(d => d.Producto)
                .Where(p => p.Estado == estado)
                .ToListAsync();

            // Obtener productos para cada proveedor
            foreach (var proveedor in proveedores)
            {
                await ObtenerProductosProveedor(proveedor);
            }

            return proveedores;
        }

        public async Task<List<Proveedor>> SelectWithRelations()
        {
            var proveedores = await _context.Proveedores
                .Include(p => p.ComprasProveedor)
                    .ThenInclude(c => c.DetallesCompra)
                        .ThenInclude(d => d.Producto)
                .ToListAsync();

            // Obtener productos para cada proveedor
            foreach (var proveedor in proveedores)
            {
                await ObtenerProductosProveedor(proveedor);
            }

            return proveedores;
        }

        public async Task<Proveedor?> SelectByIdWithRelations(int id)
        {
            var proveedor = await _context.Proveedores
                .Include(p => p.ComprasProveedor)
                    .ThenInclude(c => c.DetallesCompra)
                        .ThenInclude(d => d.Producto)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (proveedor != null)
            {
                await ObtenerProductosProveedor(proveedor);
            }

            return proveedor;
        }

        private async Task ObtenerProductosProveedor(Proveedor proveedor)
        {
            // Obtener todos los productos únicos del proveedor
            var productos = proveedor.ComprasProveedor
                .SelectMany(c => c.DetallesCompra)
                .Where(d => d.Producto != null)
                .Select(d => d.Producto.Descripcion)
                .Distinct()
                .ToList();

            // Guardar productos en Notas temporalmente (se mapeará a Productos en el DTO)
            proveedor.Notas = string.Join(", ", productos);
        }

        public async Task<bool> Existe(int id)
        {
            try
            {
                return await _context.Proveedores.AnyAsync(p => p.Id == id);
            }
            catch
            {
                return false;
            }
        }

        public async Task<int> Insert(Proveedor proveedor)
        {
            try
            {
                _context.Proveedores.Add(proveedor);
                await _context.SaveChangesAsync();
                return proveedor.Id;
            }
            catch
            {
                return -1;
            }
        }

        public async Task<bool> Update(int id, Proveedor proveedor)
        {
            try
            {
                var proveedorExistente = await _context.Proveedores.FindAsync(id);
                if (proveedorExistente == null)
                    return false;

                // Actualizar propiedades
                proveedorExistente.Nombre = proveedor.Nombre;
                proveedorExistente.CUIT = proveedor.CUIT;
                proveedorExistente.Telefono = proveedor.Telefono;
                proveedorExistente.Email = proveedor.Email;
                proveedorExistente.Direccion = proveedor.Direccion;
                proveedorExistente.Estado = proveedor.Estado;
                proveedorExistente.Notas = proveedor.Notas;

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
                var proveedor = await _context.Proveedores.FindAsync(id);
                if (proveedor == null)
                    return false;

                _context.Proveedores.Remove(proveedor);
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