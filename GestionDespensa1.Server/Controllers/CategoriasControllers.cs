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
    [Route("api/Categorias")]
    public class CategoriasControllers : ControllerBase
    {
        private readonly ICategoriaRepositorio _repositorio;
        private readonly IMapper _mapper;
        private readonly Context _context;

        public CategoriasControllers(ICategoriaRepositorio repositorio, IMapper mapper, Context context)
        {
            _repositorio = repositorio;
            _mapper = mapper;
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<CategoriaDTO>>> Get()
        {
            try
            {
                Console.WriteLine("🔍 Iniciando GET /api/Categorias");

                var categorias = await _repositorio.SelectWithRelations();
                Console.WriteLine($"✅ Categorías encontradas: {categorias?.Count ?? 0}");

                if (categorias == null || !categorias.Any())
                {
                    Console.WriteLine("ℹ️  No hay categorías, retornando lista vacía");
                    return new List<CategoriaDTO>();
                }

                var categoriasDTO = _mapper.Map<List<CategoriaDTO>>(categorias);
                Console.WriteLine($"✅ Mapeo exitoso. DTOs: {categoriasDTO.Count}");

                return Ok(categoriasDTO);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error en GET Categorías: {ex.Message}");
                Console.WriteLine($"📋 Stack: {ex.StackTrace}");
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"🔍 Inner: {ex.InnerException.Message}");
                }
                return StatusCode(500, $"Error al cargar categorías: {ex.Message}");
            }
        }

        
        [HttpGet("test")]
        public ActionResult<string> Test()
        {
            return "✅ Controller de Categorías funcionando correctamente";
        }

        [HttpGet("count")]
        public async Task<ActionResult<int>> GetCount()
        {
            try
            {
                var count = await _context.Categorias.CountAsync();
                Console.WriteLine($"📊 Total categorías en BD: {count}");
                return count;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error en count: {ex.Message}");
                return BadRequest($"Error count: {ex.Message}");
            }
        }

        [HttpGet("simple")]
        public async Task<ActionResult<List<CategoriaDTO>>> GetSimple()
        {
            try
            {
                var categorias = await _context.Categorias
                    .Select(c => new CategoriaDTO
                    {
                        Id = c.Id,
                        NombreCategoria = c.NombreCategoria,
                        
                    })
                    .ToListAsync();

                Console.WriteLine($"✅ GetSimple exitoso. Categorías: {categorias.Count}");
                return Ok(categorias);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error en GetSimple: {ex.Message}");
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<CategoriaDTO>> Get(int id)
        {
            try
            {
                Console.WriteLine($"🔍 Buscando categoría ID: {id}");

                var categoria = await _repositorio.SelectByIdWithRelations(id);
                if (categoria == null)
                {
                    Console.WriteLine($"❌ Categoría {id} no encontrada");
                    return NotFound();
                }

                var categoriaDTO = _mapper.Map<CategoriaDTO>(categoria);
                Console.WriteLine($"✅ Categoría encontrada: {categoriaDTO.NombreCategoria}");
                return Ok(categoriaDTO);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error en GET {id}: {ex.Message}");
                return BadRequest($"Error: {ex.Message}");
            }
        }

        [HttpGet("GetByNombre/{nombre}")]
        public async Task<ActionResult<CategoriaDTO>> GetByNombre(string nombre)
        {
            try
            {
                Console.WriteLine($"🔍 Buscando categoría por nombre: {nombre}");

                var categoria = await _repositorio.GetByNombre(nombre);
                if (categoria == null)
                {
                    Console.WriteLine($"❌ Categoría '{nombre}' no encontrada");
                    return NotFound();
                }

                var categoriaDTO = _mapper.Map<CategoriaDTO>(categoria);
                Console.WriteLine($"✅ Categoría encontrada: {categoriaDTO.NombreCategoria}");
                return Ok(categoriaDTO);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error en GetByNombre {nombre}: {ex.Message}");
                return BadRequest($"Error: {ex.Message}");
            }
        }

        [HttpGet("existe/{id:int}")]
        public async Task<ActionResult<bool>> Existe(int id)
        {
            try
            {
                var existe = await _repositorio.Existe(id);
                Console.WriteLine($"✅ Existe categoría {id}: {existe}");
                return existe;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error en Existe {id}: {ex.Message}");
                return BadRequest($"Error: {ex.Message}");
            }
        }

        [HttpPost]
        public async Task<ActionResult<int>> Post(CrearCategoriaDTO crearCategoriaDTO)
        {
            try
            {
                Console.WriteLine($"📝 Intentando crear categoría: {crearCategoriaDTO.NombreCategoria}");

                var categoria = _mapper.Map<Categoria>(crearCategoriaDTO);
                var idCreado = await _repositorio.Insert(categoria);

                if (idCreado == -1)
                {
                    Console.WriteLine($"❌ Error al crear categoría");
                    return BadRequest("No se pudo crear la categoría");
                }

                Console.WriteLine($"✅ Categoría creada con ID: {idCreado}");
                return idCreado;
            }
            catch (Exception err)
            {
                Console.WriteLine($"❌ Error en POST Categoría: {err.Message}");
                return BadRequest(err.Message);
            }
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(int id, CategoriaDTO categoriaDTO)
        {
            try
            {
                Console.WriteLine($"✏️ Intentando actualizar categoría {id}");

                if (id != categoriaDTO.Id)
                {
                    Console.WriteLine($"❌ IDs no coinciden: {id} vs {categoriaDTO.Id}");
                    return BadRequest("Datos Incorrectos");
                }

                var categoria = _mapper.Map<Categoria>(categoriaDTO);
                var resultado = await _repositorio.Update(id, categoria);

                if (!resultado)
                {
                    Console.WriteLine($"❌ No se pudo actualizar categoría {id}");
                    return BadRequest("No se pudo actualizar la categoría");
                }

                Console.WriteLine($"✅ Categoría {id} actualizada correctamente");
                return Ok();
            }
            catch (Exception e)
            {
                Console.WriteLine($"❌ Error en PUT Categoría {id}: {e.Message}");
                return BadRequest(e.Message);
            }
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                Console.WriteLine($"🗑️ Intentando eliminar categoría {id}");

                var resp = await _repositorio.Delete(id);
                if (!resp)
                {
                    Console.WriteLine($"❌ No se pudo borrar categoría {id}");
                    return BadRequest("La categoría no se pudo borrar");
                }

                Console.WriteLine($"✅ Categoría {id} eliminada correctamente");
                return Ok();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error en DELETE Categoría {id}: {ex.Message}");
                return BadRequest($"Error: {ex.Message}");
            }
        }
    }
}