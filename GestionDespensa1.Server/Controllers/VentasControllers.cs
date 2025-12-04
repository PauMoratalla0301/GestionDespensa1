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
            try
            {
                Console.WriteLine($"📝 Intentando crear venta para cliente: {crearVentaDTO.IdCliente}");

                var venta = _mapper.Map<Venta>(crearVentaDTO);
                var idCreado = await _repositorio.Insert(venta);

                if (idCreado == -1)
                {
                    Console.WriteLine($"❌ Error al crear venta");
                    return BadRequest("No se pudo crear la venta");
                }

                Console.WriteLine($"✅ Venta creada con ID: {idCreado}");
                return idCreado;
            }
            catch (Exception err)
            {
                Console.WriteLine($"❌ Error en POST Venta: {err.Message}");
                return BadRequest(err.Message);
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
                Console.WriteLine($"🗑️ Intentando eliminar venta {id}");

                var resp = await _repositorio.Delete(id);
                if (!resp)
                {
                    Console.WriteLine($"❌ No se pudo borrar venta {id}");
                    return BadRequest("La venta no se pudo borrar");
                }

                Console.WriteLine($"✅ Venta {id} eliminada correctamente");
                return Ok();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error en DELETE Venta {id}: {ex.Message}");
                return BadRequest($"Error: {ex.Message}");
            }
        }
    }
}