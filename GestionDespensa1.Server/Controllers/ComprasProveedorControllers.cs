using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GestionDespensa1.BD.Data;
using GestionDespensa1.BD.Data.Entity;
using GestionDespensa1.Server.Repositorio;
using AutoMapper;
using GestionDespensa1.Shared.DTO;

namespace GestionDespensa1.Server.Controllers
{
    [ApiController]
    [Route("api/ComprasProveedor")]
    public class ComprasProveedorControllers : ControllerBase
    {
        private readonly ICompraProveedorRepositorio _repositorio;
        private readonly IMapper _mapper;
        private readonly Context _context;

        public ComprasProveedorControllers(ICompraProveedorRepositorio repositorio, IMapper mapper, Context context)
        {
            _repositorio = repositorio;
            _mapper = mapper;
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<CompraProveedorDTO>>> Get()
        {
            try
            {
                Console.WriteLine("🔍 Iniciando GET /api/ComprasProveedor");

                var compras = await _repositorio.SelectWithRelations();
                Console.WriteLine($"✅ Compras encontradas: {compras?.Count ?? 0}");

                if (compras == null || !compras.Any())
                {
                    Console.WriteLine("ℹ️  No hay compras, retornando lista vacía");
                    return new List<CompraProveedorDTO>();
                }

                var comprasDTO = _mapper.Map<List<CompraProveedorDTO>>(compras);
                Console.WriteLine($"✅ Mapeo exitoso. DTOs: {comprasDTO.Count}");

                return Ok(comprasDTO);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error en GET ComprasProveedor: {ex.Message}");
                Console.WriteLine($"📋 Stack: {ex.StackTrace}");
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"🔍 Inner: {ex.InnerException.Message}");
                }
                return StatusCode(500, $"Error al cargar compras: {ex.Message}");
            }
        }

        // Endpoints de diagnóstico
        [HttpGet("test")]
        public ActionResult<string> Test()
        {
            return "✅ Controller de ComprasProveedor funcionando correctamente";
        }

        [HttpGet("count")]
        public async Task<ActionResult<int>> GetCount()
        {
            try
            {
                var count = await _context.ComprasProveedor.CountAsync();
                Console.WriteLine($"📊 Total compras en BD: {count}");
                return count;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error en count: {ex.Message}");
                return BadRequest($"Error count: {ex.Message}");
            }
        }

        [HttpGet("simple")]
        public async Task<ActionResult<List<CompraProveedorDTO>>> GetSimple()
        {
            try
            {
                var compras = await _context.ComprasProveedor
                    .Include(c => c.Proveedor)
                    .Select(c => new CompraProveedorDTO
                    {
                        Id = c.Id,
                        IdProveedor = c.IdProveedor,
                        FechaCompra = c.FechaCompra,
                        Observaciones = c.Observaciones,
                        Proveedor = c.Proveedor != null ? new ProveedorDTO
                        {
                            Id = c.Proveedor.Id,
                            Nombre = c.Proveedor.Nombre,
                            // otras propiedades de Proveedor
                        } : null,
                        DetallesCompra = new List<DetalleCompraProveedorDTO>() // Lista vacía por ahora
                    })
                    .ToListAsync();

                Console.WriteLine($"✅ GetSimple exitoso. Compras: {compras.Count}");
                return Ok(compras);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error en GetSimple: {ex.Message}");
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<CompraProveedorDTO>> Get(int id)
        {
            try
            {
                Console.WriteLine($"🔍 Buscando compra ID: {id}");

                var compra = await _repositorio.SelectByIdWithRelations(id);
                if (compra == null)
                {
                    Console.WriteLine($"❌ Compra {id} no encontrada");
                    return NotFound();
                }

                var compraDTO = _mapper.Map<CompraProveedorDTO>(compra);
                Console.WriteLine($"✅ Compra encontrada: ID {compraDTO.Id}");
                return Ok(compraDTO);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error en GET {id}: {ex.Message}");
                return BadRequest($"Error: {ex.Message}");
            }
        }

        [HttpGet("GetByProveedor/{proveedorId:int}")]
        public async Task<ActionResult<List<CompraProveedorDTO>>> GetByProveedor(int proveedorId)
        {
            try
            {
                Console.WriteLine($"🔍 Buscando compras por proveedor: {proveedorId}");

                var compras = await _repositorio.GetByProveedor(proveedorId);
                var comprasDTO = _mapper.Map<List<CompraProveedorDTO>>(compras);

                Console.WriteLine($"✅ Compras encontradas para proveedor {proveedorId}: {comprasDTO.Count}");
                return Ok(comprasDTO);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error en GetByProveedor {proveedorId}: {ex.Message}");
                return BadRequest($"Error: {ex.Message}");
            }
        }

        [HttpGet("GetByFecha/{fecha}")]
        public async Task<ActionResult<List<CompraProveedorDTO>>> GetByFecha(string fecha)
        {
            try
            {
                Console.WriteLine($"🔍 Buscando compras por fecha: {fecha}");

                var compras = await _repositorio.GetByFecha(fecha);
                var comprasDTO = _mapper.Map<List<CompraProveedorDTO>>(compras);

                Console.WriteLine($"✅ Compras encontradas para fecha {fecha}: {comprasDTO.Count}");
                return Ok(comprasDTO);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error en GetByFecha {fecha}: {ex.Message}");
                return BadRequest($"Error: {ex.Message}");
            }
        }

        [HttpGet("existe/{id:int}")]
        public async Task<ActionResult<bool>> Existe(int id)
        {
            try
            {
                var existe = await _repositorio.Existe(id);
                Console.WriteLine($"✅ Existe compra {id}: {existe}");
                return existe;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error en Existe {id}: {ex.Message}");
                return BadRequest($"Error: {ex.Message}");
            }
        }

        [HttpPost]
        public async Task<ActionResult<int>> Post(CrearCompraProveedorDTO crearCompraProveedorDTO)
        {
            try
            {
                Console.WriteLine($"📝 Intentando crear compra para proveedor: {crearCompraProveedorDTO.IdProveedor}");

                var compra = _mapper.Map<CompraProveedor>(crearCompraProveedorDTO);
                var idCreado = await _repositorio.Insert(compra);

                if (idCreado == -1)
                {
                    Console.WriteLine($"❌ Error al crear compra");
                    return BadRequest("No se pudo crear la compra de proveedor");
                }

                Console.WriteLine($"✅ Compra creada con ID: {idCreado}");
                return idCreado;
            }
            catch (Exception err)
            {
                Console.WriteLine($"❌ Error en POST CompraProveedor: {err.Message}");
                return BadRequest(err.Message);
            }
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(int id, CompraProveedorDTO compraProveedorDTO)
        {
            try
            {
                Console.WriteLine($"✏️ Intentando actualizar compra {id}");

                if (id != compraProveedorDTO.Id)
                {
                    Console.WriteLine($"❌ IDs no coinciden: {id} vs {compraProveedorDTO.Id}");
                    return BadRequest("Datos Incorrectos");
                }

                var compra = _mapper.Map<CompraProveedor>(compraProveedorDTO);
                var resultado = await _repositorio.Update(id, compra);

                if (!resultado)
                {
                    Console.WriteLine($"❌ No se pudo actualizar compra {id}");
                    return BadRequest("No se pudo actualizar la compra de proveedor");
                }

                Console.WriteLine($"✅ Compra {id} actualizada correctamente");
                return Ok();
            }
            catch (Exception e)
            {
                Console.WriteLine($"❌ Error en PUT CompraProveedor {id}: {e.Message}");
                return BadRequest(e.Message);
            }
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                Console.WriteLine($"🗑️ Intentando eliminar compra {id}");

                var resp = await _repositorio.Delete(id);
                if (!resp)
                {
                    Console.WriteLine($"❌ No se pudo borrar compra {id}");
                    return BadRequest("La compra de proveedor no se pudo borrar");
                }

                Console.WriteLine($"✅ Compra {id} eliminada correctamente");
                return Ok();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error en DELETE CompraProveedor {id}: {ex.Message}");
                return BadRequest($"Error: {ex.Message}");
            }
        }
    }
}