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
                .FirstOrDefaultAsync(p => p.CUIT == cuit);
        }

        public async Task<List<Proveedor>> GetByEstado(string estado)
        {
            return await _context.Proveedores
                .Where(p => p.Estado == estado)
                .ToListAsync();
        }

        public async Task<List<Proveedor>> SelectWithRelations()
        {
            return await _context.Proveedores
                .Include(p => p.ComprasProveedor)
                .ToListAsync();
        }

        public async Task<Proveedor?> SelectByIdWithRelations(int id)
        {
            return await _context.Proveedores
                .Include(p => p.ComprasProveedor)
                .FirstOrDefaultAsync(p => p.Id == id);
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

        // ELIMINA ESTOS MÉTODOS DUPLICADOS:
        // Task<ActionResult<bool>> IProveedorRepositorio.Existe(int id)
        // Task<ActionResult<int>> IProveedorRepositorio.Insert(Proveedor proveedor)
    }
}