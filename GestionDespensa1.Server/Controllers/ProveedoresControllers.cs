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
    [Route("api/Proveedores")]
    public class ProveedoresControllers : ControllerBase
    {
        private readonly IProveedorRepositorio _repositorio;
        private readonly IMapper _mapper;
        private readonly Context _context;

        public ProveedoresControllers(IProveedorRepositorio repositorio, IMapper mapper, Context context)
        {
            _repositorio = repositorio;
            _mapper = mapper;
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<ProveedorDTO>>> Get()
        {
            try
            {
                Console.WriteLine("🔍 Iniciando GET /api/Proveedores");

                var proveedores = await _repositorio.SelectWithRelations();
                Console.WriteLine($"✅ Proveedores encontrados: {proveedores?.Count ?? 0}");

                if (proveedores == null || !proveedores.Any())
                {
                    Console.WriteLine("ℹ️  No hay proveedores, retornando lista vacía");
                    return new List<ProveedorDTO>();
                }

                var proveedoresDTO = _mapper.Map<List<ProveedorDTO>>(proveedores);
                Console.WriteLine($"✅ Mapeo exitoso. DTOs: {proveedoresDTO.Count}");

                return Ok(proveedoresDTO);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error en GET Proveedores: {ex.Message}");
                Console.WriteLine($"📋 Stack: {ex.StackTrace}");
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"🔍 Inner: {ex.InnerException.Message}");
                }
                return StatusCode(500, $"Error al cargar proveedores: {ex.Message}");
            }
        }

        // Endpoints de diagnóstico
        [HttpGet("test")]
        public ActionResult<string> Test()
        {
            return "✅ Controller de Proveedores funcionando correctamente";
        }

        [HttpGet("count")]
        public async Task<ActionResult<int>> GetCount()
        {
            try
            {
                var count = await _context.Proveedores.CountAsync();
                Console.WriteLine($"📊 Total proveedores en BD: {count}");
                return count;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error en count: {ex.Message}");
                return BadRequest($"Error count: {ex.Message}");
            }
        }

        [HttpGet("simple")]
        public async Task<ActionResult<List<ProveedorDTO>>> GetSimple()
        {
            try
            {
                var proveedores = await _context.Proveedores
                    .Select(p => new ProveedorDTO
                    {
                        Id = p.Id,
                        Nombre = p.Nombre,
                        CUIT = p.CUIT,
                        Telefono = p.Telefono,
                        Email = p.Email,
                        Direccion = p.Direccion,
                        Estado = p.Estado,
                        Notas = p.Notas
                    })
                    .ToListAsync();

                Console.WriteLine($"✅ GetSimple exitoso. Proveedores: {proveedores.Count}");
                return Ok(proveedores);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error en GetSimple: {ex.Message}");
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<ProveedorDTO>> Get(int id)
        {
            try
            {
                Console.WriteLine($"🔍 Buscando proveedor ID: {id}");

                var proveedor = await _repositorio.SelectByIdWithRelations(id);
                if (proveedor == null)
                {
                    Console.WriteLine($"❌ Proveedor {id} no encontrado");
                    return NotFound();
                }

                var proveedorDTO = _mapper.Map<ProveedorDTO>(proveedor);
                Console.WriteLine($"✅ Proveedor encontrado: {proveedorDTO.Nombre}");
                return Ok(proveedorDTO);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error en GET {id}: {ex.Message}");
                return BadRequest($"Error: {ex.Message}");
            }
        }

        [HttpGet("GetByCuit/{cuit}")]
        public async Task<ActionResult<ProveedorDTO>> GetByCuit(string cuit)
        {
            try
            {
                Console.WriteLine($"🔍 Buscando proveedor por CUIT: {cuit}");

                var proveedor = await _repositorio.GetByCuit(cuit);
                if (proveedor == null)
                {
                    Console.WriteLine($"❌ Proveedor con CUIT '{cuit}' no encontrado");
                    return NotFound();
                }

                var proveedorDTO = _mapper.Map<ProveedorDTO>(proveedor);
                Console.WriteLine($"✅ Proveedor encontrado: {proveedorDTO.Nombre}");
                return Ok(proveedorDTO);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error en GetByCuit {cuit}: {ex.Message}");
                return BadRequest($"Error: {ex.Message}");
            }
        }

        [HttpGet("GetByEstado/{estado}")]
        public async Task<ActionResult<List<ProveedorDTO>>> GetByEstado(string estado)
        {
            try
            {
                Console.WriteLine($"🔍 Buscando proveedores por estado: {estado}");

                var proveedores = await _repositorio.GetByEstado(estado);
                var proveedoresDTO = _mapper.Map<List<ProveedorDTO>>(proveedores);

                Console.WriteLine($"✅ Proveedores encontrados con estado {estado}: {proveedoresDTO.Count}");
                return Ok(proveedoresDTO);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error en GetByEstado {estado}: {ex.Message}");
                return BadRequest($"Error: {ex.Message}");
            }
        }

        [HttpGet("existe/{id:int}")]
        public async Task<ActionResult<bool>> Existe(int id)
        {
            try
            {
                var existe = await _repositorio.Existe(id);
                Console.WriteLine($"✅ Existe proveedor {id}: {existe}");
                return existe;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error en Existe {id}: {ex.Message}");
                return BadRequest($"Error: {ex.Message}");
            }
        }

        [HttpPost]
        public async Task<ActionResult<int>> Post(CrearProveedorDTO crearProveedorDTO)
        {
            try
            {
                Console.WriteLine($"📝 Intentando crear proveedor: {crearProveedorDTO.Nombre}");

                var proveedor = _mapper.Map<Proveedor>(crearProveedorDTO);
                var idCreado = await _repositorio.Insert(proveedor);

                if (idCreado == -1)
                {
                    Console.WriteLine($"❌ Error al crear proveedor");
                    return BadRequest("No se pudo crear el proveedor");
                }

                Console.WriteLine($"✅ Proveedor creado con ID: {idCreado}");
                return idCreado;
            }
            catch (Exception err)
            {
                Console.WriteLine($"❌ Error en POST Proveedor: {err.Message}");
                return BadRequest(err.Message);
            }
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(int id, ProveedorDTO proveedorDTO)
        {
            try
            {
                Console.WriteLine($"✏️ Intentando actualizar proveedor {id}");

                if (id != proveedorDTO.Id)
                {
                    Console.WriteLine($"❌ IDs no coinciden: {id} vs {proveedorDTO.Id}");
                    return BadRequest("Datos Incorrectos");
                }

                var proveedor = _mapper.Map<Proveedor>(proveedorDTO);
                var resultado = await _repositorio.Update(id, proveedor);

                if (!resultado)
                {
                    Console.WriteLine($"❌ No se pudo actualizar proveedor {id}");
                    return BadRequest("No se pudo actualizar el proveedor");
                }

                Console.WriteLine($"✅ Proveedor {id} actualizado correctamente");
                return Ok();
            }
            catch (Exception e)
            {
                Console.WriteLine($"❌ Error en PUT Proveedor {id}: {e.Message}");
                return BadRequest(e.Message);
            }
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                Console.WriteLine($"🗑️ Intentando eliminar proveedor {id}");

                var resp = await _repositorio.Delete(id);
                if (!resp)
                {
                    Console.WriteLine($"❌ No se pudo borrar proveedor {id}");
                    return BadRequest("El proveedor no se pudo borrar");
                }

                Console.WriteLine($"✅ Proveedor {id} eliminado correctamente");
                return Ok();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error en DELETE Proveedor {id}: {ex.Message}");
                return BadRequest($"Error: {ex.Message}");
            }
        }
    }
}