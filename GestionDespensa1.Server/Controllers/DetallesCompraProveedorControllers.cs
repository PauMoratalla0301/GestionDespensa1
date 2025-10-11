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
    [Route("api/DetallesCompraProveedor")]
    public class DetallesCompraProveedorControllers : ControllerBase
    {
        private readonly IDetalleCompraProveedorRepositorio _repositorio;
        private readonly IMapper _mapper;
        private readonly Context _context;

        public DetallesCompraProveedorControllers(IDetalleCompraProveedorRepositorio repositorio, IMapper mapper, Context context)
        {
            _repositorio = repositorio;
            _mapper = mapper;
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<DetalleCompraProveedorDTO>>> Get()
        {
            try
            {
                Console.WriteLine("🔍 Iniciando GET /api/DetallesCompraProveedor");

                var detalles = await _repositorio.SelectWithRelations();
                Console.WriteLine($"✅ Detalles de compra encontrados: {detalles?.Count ?? 0}");

                if (detalles == null || !detalles.Any())
                {
                    Console.WriteLine("ℹ️  No hay detalles de compra, retornando lista vacía");
                    return new List<DetalleCompraProveedorDTO>();
                }

                var detallesDTO = _mapper.Map<List<DetalleCompraProveedorDTO>>(detalles);
                Console.WriteLine($"✅ Mapeo exitoso. DTOs: {detallesDTO.Count}");

                return Ok(detallesDTO);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error en GET DetallesCompraProveedor: {ex.Message}");
                Console.WriteLine($"📋 Stack: {ex.StackTrace}");
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"🔍 Inner: {ex.InnerException.Message}");
                }
                return StatusCode(500, $"Error al cargar detalles de compra: {ex.Message}");
            }
        }

        // Endpoints de diagnóstico
        [HttpGet("test")]
        public ActionResult<string> Test()
        {
            return "✅ Controller de DetallesCompraProveedor funcionando correctamente";
        }

        [HttpGet("count")]
        public async Task<ActionResult<int>> GetCount()
        {
            try
            {
                var count = await _context.DetallesCompraProveedor.CountAsync();
                Console.WriteLine($"📊 Total detalles de compra en BD: {count}");
                return count;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error en count: {ex.Message}");
                return BadRequest($"Error count: {ex.Message}");
            }
        }

        [HttpGet("simple")]
        public async Task<ActionResult<List<DetalleCompraProveedorDTO>>> GetSimple()
        {
            try
            {
                var detalles = await _context.DetallesCompraProveedor
                    .Select(d => new DetalleCompraProveedorDTO
                    {
                        Id = d.Id,
                        IdCompra = d.IdCompra,
                        IdProducto = d.IdProducto,
                        Cantidad = d.Cantidad,
                        PrecioUnitario = d.PrecioUnitario
                    })
                    .ToListAsync();

                Console.WriteLine($"✅ GetSimple exitoso. Detalles: {detalles.Count}");
                return Ok(detalles);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error en GetSimple: {ex.Message}");
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<DetalleCompraProveedorDTO>> Get(int id)
        {
            try
            {
                Console.WriteLine($"🔍 Buscando detalle de compra ID: {id}");

                var detalle = await _repositorio.SelectByIdWithRelations(id);
                if (detalle == null)
                {
                    Console.WriteLine($"❌ Detalle de compra {id} no encontrado");
                    return NotFound();
                }

                var detalleDTO = _mapper.Map<DetalleCompraProveedorDTO>(detalle);
                Console.WriteLine($"✅ Detalle encontrado: ID {detalleDTO.Id}");
                return Ok(detalleDTO);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error en GET {id}: {ex.Message}");
                return BadRequest($"Error: {ex.Message}");
            }
        }

        [HttpGet("GetByCompra/{compraId:int}")]
        public async Task<ActionResult<List<DetalleCompraProveedorDTO>>> GetByCompra(int compraId)
        {
            try
            {
                Console.WriteLine($"🔍 Buscando detalles por compra: {compraId}");

                var detalles = await _repositorio.GetByCompra(compraId);
                var detallesDTO = _mapper.Map<List<DetalleCompraProveedorDTO>>(detalles);

                Console.WriteLine($"✅ Detalles encontrados para compra {compraId}: {detallesDTO.Count}");
                return Ok(detallesDTO);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error en GetByCompra {compraId}: {ex.Message}");
                return BadRequest($"Error: {ex.Message}");
            }
        }

        [HttpGet("GetByProducto/{productoId:int}")]
        public async Task<ActionResult<List<DetalleCompraProveedorDTO>>> GetByProducto(int productoId)
        {
            try
            {
                Console.WriteLine($"🔍 Buscando detalles por producto: {productoId}");

                var detalles = await _repositorio.GetByProducto(productoId);
                var detallesDTO = _mapper.Map<List<DetalleCompraProveedorDTO>>(detalles);

                Console.WriteLine($"✅ Detalles encontrados para producto {productoId}: {detallesDTO.Count}");
                return Ok(detallesDTO);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error en GetByProducto {productoId}: {ex.Message}");
                return BadRequest($"Error: {ex.Message}");
            }
        }

        [HttpGet("existe/{id:int}")]
        public async Task<ActionResult<bool>> Existe(int id)
        {
            try
            {
                var existe = await _repositorio.Existe(id);
                Console.WriteLine($"✅ Existe detalle de compra {id}: {existe}");
                return existe;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error en Existe {id}: {ex.Message}");
                return BadRequest($"Error: {ex.Message}");
            }
        }

        [HttpPost]
        public async Task<ActionResult<int>> Post(CrearDetalleCompraProveedorDTO crearDetalleCompraProveedorDTO)
        {
            try
            {
                Console.WriteLine($"📝 Intentando crear detalle de compra para compra: {crearDetalleCompraProveedorDTO.IdCompra}");

                var detalle = _mapper.Map<DetalleCompraProveedor>(crearDetalleCompraProveedorDTO);
                var idCreado = await _repositorio.Insert(detalle);

                if (idCreado == -1)
                {
                    Console.WriteLine($"❌ Error al crear detalle de compra");
                    return BadRequest("No se pudo crear el detalle de compra de proveedor");
                }

                Console.WriteLine($"✅ Detalle de compra creado con ID: {idCreado}");
                return idCreado;
            }
            catch (Exception err)
            {
                Console.WriteLine($"❌ Error en POST DetalleCompraProveedor: {err.Message}");
                return BadRequest(err.Message);
            }
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(int id, DetalleCompraProveedorDTO detalleCompraProveedorDTO)
        {
            try
            {
                Console.WriteLine($"✏️ Intentando actualizar detalle de compra {id}");

                if (id != detalleCompraProveedorDTO.Id)
                {
                    Console.WriteLine($"❌ IDs no coinciden: {id} vs {detalleCompraProveedorDTO.Id}");
                    return BadRequest("Datos Incorrectos");
                }

                var detalle = _mapper.Map<DetalleCompraProveedor>(detalleCompraProveedorDTO);
                var resultado = await _repositorio.Update(id, detalle);

                if (!resultado)
                {
                    Console.WriteLine($"❌ No se pudo actualizar detalle de compra {id}");
                    return BadRequest("No se pudo actualizar el detalle de compra de proveedor");
                }

                Console.WriteLine($"✅ Detalle de compra {id} actualizado correctamente");
                return Ok();
            }
            catch (Exception e)
            {
                Console.WriteLine($"❌ Error en PUT DetalleCompraProveedor {id}: {e.Message}");
                return BadRequest(e.Message);
            }
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                Console.WriteLine($"🗑️ Intentando eliminar detalle de compra {id}");

                var resp = await _repositorio.Delete(id);
                if (!resp)
                {
                    Console.WriteLine($"❌ No se pudo borrar detalle de compra {id}");
                    return BadRequest("El detalle de compra de proveedor no se pudo borrar");
                }

                Console.WriteLine($"✅ Detalle de compra {id} eliminado correctamente");
                return Ok();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error en DELETE DetalleCompraProveedor {id}: {ex.Message}");
                return BadRequest($"Error: {ex.Message}");
            }
        }
    }
}