using Microsoft.EntityFrameworkCore;
using GestionDespensa1.BD.Data;
using GestionDespensa1.BD.Data.Entity;
using Microsoft.AspNetCore.Mvc;

namespace GestionDespensa1.Server.Repositorio
{
    public class ClienteRepositorio : Repositorio<Cliente>, IClienteRepositorio
    {
        private readonly Context _context;

        public ClienteRepositorio(Context context) : base(context)
        {
            _context = context;
        }

        public async Task<Cliente?> GetByDni(string dni)
        {
            return await _context.Clientes
                .Include(c => c.Ventas)
                .FirstOrDefaultAsync(c => c.Dni == dni);
        }

        public async Task<List<Cliente>> GetBySaldoPendiente()
        {
            return await _context.Clientes
                .Where(c => c.SaldoPendiente > 0)
                .ToListAsync();
        }

        public async Task<List<Cliente>> SelectWithRelations()
        {
            return await _context.Clientes
                .Include(c => c.Ventas)
                .ToListAsync();
        }

        public async Task<Cliente?> SelectByIdWithRelations(int id)
        {
            return await _context.Clientes
                .Include(c => c.Ventas)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        // **IMPLEMENTACIONES FALTANTES**
        public async Task<ActionResult<bool>> Existe(int id)
        {
            try
            {
                var existe = await _context.Clientes.AnyAsync(c => c.Id == id);
                return new ActionResult<bool>(existe);
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(ex.Message);
            }
        }

        public async Task<ActionResult<int>> Insert(Cliente cliente)
        {
            try
            {
                _context.Clientes.Add(cliente);
                await _context.SaveChangesAsync();
                return new ActionResult<int>(cliente.Id);
            }
            catch (Exception ex)
            {
                return new BadRequestObjectResult(ex.Message);
            }
        }

        // **SI FALTAN ESTOS MÉTODOS TAMBIÉN, AÑÁDELOS:**
        public async Task<bool> Update(int id, Cliente cliente)
        {
            try
            {
                var clienteExistente = await _context.Clientes.FindAsync(id);
                if (clienteExistente == null)
                    return false;

                // Actualizar propiedades del cliente
                clienteExistente.Nombre = cliente.Nombre;
                clienteExistente.Apellido = cliente.Apellido;
                clienteExistente.Dni = cliente.Dni;
                clienteExistente.Telefono = cliente.Telefono;
                clienteExistente.Email = cliente.Email;
                clienteExistente.Direccion = cliente.Direccion;
                clienteExistente.SaldoPendiente = cliente.SaldoPendiente;

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
                var cliente = await _context.Clientes.FindAsync(id);
                if (cliente == null)
                    return false;

                _context.Clientes.Remove(cliente);
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
