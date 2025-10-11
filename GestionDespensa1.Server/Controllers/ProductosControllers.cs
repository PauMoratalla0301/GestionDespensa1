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
    [Route("api/Productos")]
    public class ProductosControllers : ControllerBase
    {
        private readonly IProductoRepositorio _repositorio;
        private readonly IMapper _mapper;
        private readonly Context _context;

        public ProductosControllers(IProductoRepositorio repositorio, IMapper mapper, Context context)
        {
            _repositorio = repositorio;
            _mapper = mapper;
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<ProductoDTO>>> Get()
        {
            try
            {
                Console.WriteLine("🔍 Iniciando GET /api/Productos");

                var productos = await _repositorio.SelectWithRelations();
                Console.WriteLine($"✅ Productos encontrados: {productos?.Count ?? 0}");

                if (productos == null || !productos.Any())
                {
                    Console.WriteLine("ℹ️  No hay productos, retornando lista vacía");
                    return new List<ProductoDTO>();
                }

                var productosDTO = _mapper.Map<List<ProductoDTO>>(productos);
                Console.WriteLine($"✅ Mapeo exitoso. DTOs: {productosDTO.Count}");

                return Ok(productosDTO);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error en GET Productos: {ex.Message}");
                Console.WriteLine($"📋 Stack: {ex.StackTrace}");
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"🔍 Inner: {ex.InnerException.Message}");
                }
                return StatusCode(500, $"Error al cargar productos: {ex.Message}");
            }
        }

        // Endpoints de diagnóstico
        [HttpGet("test")]
        public ActionResult<string> Test()
        {
            return "✅ Controller de Productos funcionando correctamente";
        }

        [HttpGet("count")]
        public async Task<ActionResult<int>> GetCount()
        {
            try
            {
                var count = await _context.Productos.CountAsync();
                Console.WriteLine($"📊 Total productos en BD: {count}");
                return count;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error en count: {ex.Message}");
                return BadRequest($"Error count: {ex.Message}");
            }
        }

        [HttpGet("simple")]
        public async Task<ActionResult<List<ProductoDTO>>> GetSimple()
        {
            try
            {
                var productos = await _context.Productos
                    .Include(p => p.Categoria)
                    .Select(p => new ProductoDTO
                    {
                        Id = p.Id,
                        Descripcion = p.Descripcion,
                        PrecioUnitario = p.PrecioUnitario,
                        GananciaPorcentaje = p.GananciaPorcentaje,
                        StockActual = p.StockActual,
                        StockMinimo = p.StockMinimo,
                        IdCategoria = p.IdCategoria,
                        Categoria = p.Categoria != null ? new CategoriaDTO
                        {
                            Id = p.Categoria.Id,
                            NombreCategoria = p.Categoria.NombreCategoria
                        } : null
                    })
                    .ToListAsync();

                Console.WriteLine($"✅ GetSimple exitoso. Productos: {productos.Count}");
                return Ok(productos);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error en GetSimple: {ex.Message}");
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<ProductoDTO>> Get(int id)
        {
            try
            {
                Console.WriteLine($"🔍 Buscando producto ID: {id}");

                var producto = await _repositorio.SelectByIdWithRelations(id);
                if (producto == null)
                {
                    Console.WriteLine($"❌ Producto {id} no encontrado");
                    return NotFound();
                }

                var productoDTO = _mapper.Map<ProductoDTO>(producto);
                Console.WriteLine($"✅ Producto encontrado: {productoDTO.Descripcion}");
                return Ok(productoDTO);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error en GET {id}: {ex.Message}");
                return BadRequest($"Error: {ex.Message}");
            }
        }

        [HttpGet("GetByDescripcion/{descripcion}")]
        public async Task<ActionResult<ProductoDTO>> GetByDescripcion(string descripcion)
        {
            try
            {
                Console.WriteLine($"🔍 Buscando producto por descripción: {descripcion}");

                var producto = await _repositorio.GetByDescripcion(descripcion);
                if (producto == null)
                {
                    Console.WriteLine($"❌ Producto '{descripcion}' no encontrado");
                    return NotFound();
                }

                var productoDTO = _mapper.Map<ProductoDTO>(producto);
                Console.WriteLine($"✅ Producto encontrado: {productoDTO.Descripcion}");
                return Ok(productoDTO);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error en GetByDescripcion {descripcion}: {ex.Message}");
                return BadRequest($"Error: {ex.Message}");
            }
        }

        [HttpGet("GetByCategoria/{categoriaId:int}")]
        public async Task<ActionResult<List<ProductoDTO>>> GetByCategoria(int categoriaId)
        {
            try
            {
                Console.WriteLine($"🔍 Buscando productos por categoría: {categoriaId}");

                var productos = await _repositorio.GetByCategoria(categoriaId);
                var productosDTO = _mapper.Map<List<ProductoDTO>>(productos);

                Console.WriteLine($"✅ Productos encontrados para categoría {categoriaId}: {productosDTO.Count}");
                return Ok(productosDTO);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error en GetByCategoria {categoriaId}: {ex.Message}");
                return BadRequest($"Error: {ex.Message}");
            }
        }

        [HttpGet("GetStockBajo")]
        public async Task<ActionResult<List<ProductoDTO>>> GetStockBajo()
        {
            try
            {
                Console.WriteLine($"🔍 Buscando productos con stock bajo");

                var productos = await _repositorio.GetStockBajo();
                var productosDTO = _mapper.Map<List<ProductoDTO>>(productos);

                Console.WriteLine($"✅ Productos con stock bajo encontrados: {productosDTO.Count}");
                return Ok(productosDTO);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error en GetStockBajo: {ex.Message}");
                return BadRequest($"Error: {ex.Message}");
            }
        }

        [HttpGet("existe/{id:int}")]
        public async Task<ActionResult<bool>> Existe(int id)
        {
            try
            {
                var existe = await _repositorio.Existe(id);
                Console.WriteLine($"✅ Existe producto {id}: {existe}");
                return existe;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error en Existe {id}: {ex.Message}");
                return BadRequest($"Error: {ex.Message}");
            }
        }

        [HttpPost]
        public async Task<ActionResult<int>> Post(CrearProductoDTO crearProductoDTO)
        {
            try
            {
                Console.WriteLine($"📝 Intentando crear producto: {crearProductoDTO.Descripcion}");

                var producto = _mapper.Map<Producto>(crearProductoDTO);
                var idCreado = await _repositorio.Insert(producto);

                if (idCreado == -1)
                {
                    Console.WriteLine($"❌ Error al crear producto");
                    return BadRequest("No se pudo crear el producto");
                }

                Console.WriteLine($"✅ Producto creado con ID: {idCreado}");
                return idCreado;
            }
            catch (Exception err)
            {
                Console.WriteLine($"❌ Error en POST Producto: {err.Message}");
                return BadRequest(err.Message);
            }
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(int id, ProductoDTO productoDTO)
        {
            try
            {
                Console.WriteLine($"✏️ Intentando actualizar producto {id}");

                if (id != productoDTO.Id)
                {
                    Console.WriteLine($"❌ IDs no coinciden: {id} vs {productoDTO.Id}");
                    return BadRequest("Datos Incorrectos");
                }

                var producto = _mapper.Map<Producto>(productoDTO);
                var resultado = await _repositorio.Update(id, producto);

                if (!resultado)
                {
                    Console.WriteLine($"❌ No se pudo actualizar producto {id}");
                    return BadRequest("No se pudo actualizar el producto");
                }

                Console.WriteLine($"✅ Producto {id} actualizado correctamente");
                return Ok();
            }
            catch (Exception e)
            {
                Console.WriteLine($"❌ Error en PUT Producto {id}: {e.Message}");
                return BadRequest(e.Message);
            }
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                Console.WriteLine($"🗑️ Intentando eliminar producto {id}");

                var resp = await _repositorio.Delete(id);
                if (!resp)
                {
                    Console.WriteLine($"❌ No se pudo borrar producto {id}");
                    return BadRequest("El producto no se pudo borrar");
                }

                Console.WriteLine($"✅ Producto {id} eliminado correctamente");
                return Ok();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error en DELETE Producto {id}: {ex.Message}");
                return BadRequest($"Error: {ex.Message}");
            }
        }
    }
}