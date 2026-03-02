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
    [Route("api/Ventas")]
    public class VentasControllers : ControllerBase
    {
        private readonly IVentaRepositorio _repositorio;
        private readonly IMapper _mapper;
        private readonly Context _context;

        public VentasControllers(IVentaRepositorio repositorio, IMapper mapper, Context context)
        {
            _repositorio = repositorio;
            _mapper = mapper;
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<VentaDTO>>> Get()
        {
            try
            {
                Console.WriteLine("🔍 Iniciando GET /api/Ventas");

                var ventas = await _repositorio.SelectWithRelations();
                Console.WriteLine($"✅ Ventas encontradas: {ventas?.Count ?? 0}");

                if (ventas == null || !ventas.Any())
                {
                    Console.WriteLine("ℹ️  No hay ventas, retornando lista vacía");
                    return new List<VentaDTO>();
                }

                var ventasDTO = _mapper.Map<List<VentaDTO>>(ventas);
                Console.WriteLine($"✅ Mapeo exitoso. DTOs: {ventasDTO.Count}");

                return Ok(ventasDTO);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error en GET Ventas: {ex.Message}");
                Console.WriteLine($"📋 Stack: {ex.StackTrace}");
                if (ex.InnerException != null)
                {
                    Console.WriteLine($"🔍 Inner: {ex.InnerException.Message}");
                }
                return StatusCode(500, $"Error al cargar ventas: {ex.Message}");
            }
        }

        // Endpoints de diagnóstico
        [HttpGet("test")]
        public ActionResult<string> Test()
        {
            return "✅ Controller de Ventas funcionando correctamente";
        }

        [HttpGet("count")]
        public async Task<ActionResult<int>> GetCount()
        {
            try
            {
                var count = await _context.Ventas.CountAsync();
                Console.WriteLine($"📊 Total ventas en BD: {count}");
                return count;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error en count: {ex.Message}");
                return BadRequest($"Error count: {ex.Message}");
            }
        }

        [HttpGet("simple")]
        public async Task<ActionResult<List<VentaDTO>>> GetSimple()
        {
            try
            {
                var ventas = await _context.Ventas
                    .Include(v => v.Cliente)
                    .Select(v => new VentaDTO
                    {
                        Id = v.Id,
                        IdCliente = v.IdCliente,
                        FechaVenta = v.FechaVenta,
                        Estado = v.Estado,
                        Total = v.Total,
                        MontoPagado = v.MontoPagado,
                        SaldoPendiente = v.SaldoPendiente
                    })
                    .ToListAsync();

                Console.WriteLine($"✅ GetSimple exitoso. Ventas: {ventas.Count}");
                return Ok(ventas);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error en GetSimple: {ex.Message}");
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<VentaDTO>> Get(int id)
        {
            try
            {
                Console.WriteLine($"🔍 Buscando venta ID: {id}");

                var venta = await _repositorio.SelectByIdWithRelations(id);
                if (venta == null)
                {
                    Console.WriteLine($"❌ Venta {id} no encontrada");
                    return NotFound();
                }

                var ventaDTO = _mapper.Map<VentaDTO>(venta);
                Console.WriteLine($"✅ Venta encontrada: ID {ventaDTO.Id}");
                return Ok(ventaDTO);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error en GET {id}: {ex.Message}");
                return BadRequest($"Error: {ex.Message}");
            }
        }

        [HttpGet("GetByCliente/{clienteId:int}")]
        public async Task<ActionResult<List<VentaDTO>>> GetByCliente(int clienteId)
        {
            try
            {
                Console.WriteLine($"🔍 Buscando ventas por cliente: {clienteId}");

                var ventas = await _repositorio.GetByCliente(clienteId);
                var ventasDTO = _mapper.Map<List<VentaDTO>>(ventas);

                Console.WriteLine($"✅ Ventas encontradas para cliente {clienteId}: {ventasDTO.Count}");
                return Ok(ventasDTO);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error en GetByCliente {clienteId}: {ex.Message}");
                return BadRequest($"Error: {ex.Message}");
            }
        }

        [HttpGet("GetByFecha/{fecha}")]
        public async Task<ActionResult<List<VentaDTO>>> GetByFecha(DateTime fecha)
        {
            try
            {
                Console.WriteLine($"🔍 Buscando ventas por fecha: {fecha:yyyy-MM-dd}");

                var ventas = await _repositorio.GetByFecha(fecha);
                var ventasDTO = _mapper.Map<List<VentaDTO>>(ventas);

                Console.WriteLine($"✅ Ventas encontradas para fecha {fecha:yyyy-MM-dd}: {ventasDTO.Count}");
                return Ok(ventasDTO);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error en GetByFecha {fecha:yyyy-MM-dd}: {ex.Message}");
                return BadRequest($"Error: {ex.Message}");
            }
        }

        [HttpGet("GetConSaldoPendiente")]
        public async Task<ActionResult<List<VentaDTO>>> GetConSaldoPendiente()
        {
            try
            {
                Console.WriteLine($"🔍 Buscando ventas con saldo pendiente");

                var ventas = await _repositorio.GetVentasConSaldoPendiente();
                var ventasDTO = _mapper.Map<List<VentaDTO>>(ventas);

                Console.WriteLine($"✅ Ventas con saldo pendiente encontradas: {ventasDTO.Count}");
                return Ok(ventasDTO);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error en GetConSaldoPendiente: {ex.Message}");
                return BadRequest($"Error: {ex.Message}");
            }
        }

        [HttpGet("existe/{id:int}")]
        public async Task<ActionResult<bool>> Existe(int id)
        {
            try
            {
                var existe = await _repositorio.Existe(id);
                Console.WriteLine($"✅ Existe venta {id}: {existe}");
                return existe;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error en Existe {id}: {ex.Message}");
                return BadRequest($"Error: {ex.Message}");
            }
        }

        [HttpPost]
        public async Task<ActionResult<int>> Post(CrearVentaDTO crearVentaDTO)
        {
            Console.WriteLine("=== JSON RECIBIDO ===");
            Console.WriteLine(System.Text.Json.JsonSerializer.Serialize(crearVentaDTO));

            var clienteExiste = await _context.Clientes
                .AnyAsync(c => c.Id == crearVentaDTO.IdCliente);

            if (!clienteExiste)
                return BadRequest($"El cliente {crearVentaDTO.IdCliente} no existe.");

            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                Console.WriteLine($"📝 Creando venta para cliente {crearVentaDTO.IdCliente}");

                var venta = new Venta
                {
                    IdCliente = crearVentaDTO.IdCliente,
                    FechaVenta = DateTime.Now,
                    MetodoPago = crearVentaDTO.MetodoPago,
                    MontoPagado = crearVentaDTO.MontoPagado,
                    DetallesVenta = new List<DetalleVenta>()
                };
                //if (venta.DetallesVenta == null || !venta.DetallesVenta.Any())
                //    return BadRequest("La venta no tiene productos.");
                foreach (var detDTO in crearVentaDTO.DetallesVenta)
                {
                    venta.DetallesVenta.Add(new DetalleVenta
                    {
                        IdProducto = detDTO.IdProducto,
                        Cantidad = detDTO.Cantidad,
                        PrecioUnitario = detDTO.PrecioUnitario
                    });
                }

                // 🔥 Recalcular total REAL desde detalles
                venta.Total = venta.DetallesVenta.Sum(d => d.Cantidad * d.PrecioUnitario);

                if (crearVentaDTO.MetodoPago == "Efectivo")
                {
                    venta.MontoPagado = venta.Total;
                }

                venta.SaldoPendiente = venta.Total - venta.MontoPagado;
                venta.Estado = venta.SaldoPendiente <= 0 ? "Pagado" : "Pendiente";

                // 🔒 Validar y descontar stock
                foreach (var det in venta.DetallesVenta)
                {
                    var producto = await _context.Productos
                        .FirstOrDefaultAsync(p => p.Id == det.IdProducto);

                    if (producto == null)
                        throw new Exception($"Producto ID {det.IdProducto} no existe.");

                    if (producto.StockActual < det.Cantidad)
                        throw new Exception($"Stock insuficiente para {producto.Descripcion}.");

                    producto.StockActual -= det.Cantidad;
                }

                _context.Ventas.Add(venta);

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                Console.WriteLine($"✅ Venta creada con ID {venta.Id}");

                return Ok(venta.Id);
            }
            catch (Exception ex)
            {
                Console.WriteLine("ERROR COMPLETO:");
                Console.WriteLine(ex.ToString());

                if (ex.InnerException != null)
                {
                    Console.WriteLine("INNER EXCEPTION:");
                    Console.WriteLine(ex.InnerException.ToString());
                }

                return BadRequest(ex.InnerException?.Message ?? ex.Message);
            }
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult> Put(int id, VentaDTO ventaDTO)
        {
            try
            {
                Console.WriteLine($"✏️ Intentando actualizar venta {id}");

                if (id != ventaDTO.Id)
                {
                    Console.WriteLine($"❌ IDs no coinciden: {id} vs {ventaDTO.Id}");
                    return BadRequest("Datos Incorrectos");
                }

                var venta = _mapper.Map<Venta>(ventaDTO);
                var resultado = await _repositorio.Update(id, venta);

                if (!resultado)
                {
                    Console.WriteLine($"❌ No se pudo actualizar venta {id}");
                    return BadRequest("No se pudo actualizar la venta");
                }

                Console.WriteLine($"✅ Venta {id} actualizada correctamente");
                return Ok();
            }
            catch (Exception e)
            {
                Console.WriteLine($"❌ Error en PUT Venta {id}: {e.Message}");
                return BadRequest(e.Message);
            }
        }
        [HttpGet("GetResumenPorFecha/{fecha}")]
        public async Task<ActionResult<ResumenVentasDTO>> GetResumenPorFecha(DateTime fecha)
        {
            try
            {
                Console.WriteLine($"📊 Buscando resumen de ventas por fecha: {fecha:yyyy-MM-dd}");

                // Obtener ventas del día usando tu repositorio
                var ventasDelDia = await _repositorio.GetByFecha(fecha);

                // Filtrar solo ventas completadas/pagadas (ajusta según tus estados)
                var ventasCompletadas = ventasDelDia
                    .Where(v => v.Estado == "Completada" ||
                               v.Estado == "Pagado" ||
                               v.Estado == "Finalizada" ||
                               v.Estado == "Entregada") // Agrega los estados que uses
                    .ToList();

                Console.WriteLine($"📈 Ventas del día: {ventasCompletadas.Count} completadas de {ventasDelDia.Count} totales");

                // Calcular resumen
                var resumen = new ResumenVentasDTO
                {
                    TotalVentas = ventasCompletadas.Sum(v => v.Total),
                    TotalEfectivo = ventasCompletadas.Where(v => v.MetodoPago == "Efectivo").Sum(v => v.Total),
                    TotalTarjeta = ventasCompletadas.Where(v => v.MetodoPago == "Tarjeta").Sum(v => v.Total),
                    TotalTransferencia = ventasCompletadas.Where(v => v.MetodoPago == "Transferencia").Sum(v => v.Total),
                    CantidadVentas = ventasCompletadas.Count,
                    ImporteInicio = 0, // Se establecerá desde el frontend
                    TotalEgresos = 0   // Se establecerá desde el frontend
                };

                // Log para debugging
                Console.WriteLine($"💰 Resumen calculado:");
                Console.WriteLine($"   - Total Ventas: {resumen.TotalVentas:C}");
                Console.WriteLine($"   - Efectivo: {resumen.TotalEfectivo:C}");
                Console.WriteLine($"   - Tarjeta: {resumen.TotalTarjeta:C}");
                Console.WriteLine($"   - Transferencia: {resumen.TotalTransferencia:C}");
                Console.WriteLine($"   - Cantidad: {resumen.CantidadVentas}");

                // Validar que la suma de medios de pago coincida con el total
                var sumaMedios = resumen.TotalEfectivo + resumen.TotalTarjeta + resumen.TotalTransferencia;
                var diferencia = Math.Abs(resumen.TotalVentas - sumaMedios);

                if (diferencia > 0.01m)
                {
                    Console.WriteLine($"⚠️  Advertencia: Diferencia en medios de pago: {diferencia:C}");
                }

                return Ok(resumen);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error en GetResumenPorFecha {fecha:yyyy-MM-dd}: {ex.Message}");
                Console.WriteLine($"📋 Stack: {ex.StackTrace}");
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }
        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                var venta = await _context.Ventas
                    .Include(v => v.DetallesVenta)
                    .Include(v => v.Pagos)
                    .FirstOrDefaultAsync(v => v.Id == id);

                if (venta == null)
                    return NotFound();

                // 🔄 Devolver stock
                var productosIds = venta.DetallesVenta.Select(d => d.IdProducto).ToList();

                var productos = await _context.Productos
                    .Where(p => productosIds.Contains(p.Id))
                    .ToListAsync();

                foreach (var det in venta.DetallesVenta)
                {
                    var producto = productos.First(p => p.Id == det.IdProducto);
                    producto.StockActual += det.Cantidad;
                }

                // Eliminar pagos
                if (venta.Pagos != null && venta.Pagos.Any())
                    _context.Pagos.RemoveRange(venta.Pagos);

                // Eliminar detalles
                if (venta.DetallesVenta != null && venta.DetallesVenta.Any())
                    _context.DetallesVenta.RemoveRange(venta.DetallesVenta);

                _context.Ventas.Remove(venta);

                await _context.SaveChangesAsync();

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}