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
        private readonly IDetalleVentaRepositorio _detalleVentaRepositorio;
        private readonly IUsuarioRepositorio _usuarioRepositorio;
        private readonly IProductoRepositorio _productoRepositorio;
        private readonly IMovimientoStockRepositorio _movimientoStockRepositorio;
        private readonly IMapper _mapper;
        private readonly Context _context;

        public VentasControllers(
            IVentaRepositorio repositorio,
            IDetalleVentaRepositorio detalleVentaRepositorio,
            IUsuarioRepositorio usuarioRepositorio,
            IProductoRepositorio productoRepositorio,
            IMovimientoStockRepositorio movimientoStockRepositorio,
            IMapper mapper,
            Context context)
        {
            _repositorio = repositorio;
            _detalleVentaRepositorio = detalleVentaRepositorio;
            _usuarioRepositorio = usuarioRepositorio;
            _productoRepositorio = productoRepositorio;
            _movimientoStockRepositorio = movimientoStockRepositorio;
            _mapper = mapper;
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<VentaDTO>>> Get()
        {
            try
            {
                var ventas = await _repositorio.SelectWithRelations();
                var ventasDTO = _mapper.Map<List<VentaDTO>>(ventas);
                return Ok(ventasDTO);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<VentaDTO>> Get(int id)
        {
            try
            {
                var venta = await _repositorio.SelectByIdWithRelations(id);
                if (venta == null)
                    return NotFound();

                var ventaDTO = _mapper.Map<VentaDTO>(venta);
                return Ok(ventaDTO);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }
        }

        [HttpGet("GetConSaldoPendiente")]
        public async Task<ActionResult<List<VentaDTO>>> GetConSaldoPendiente()
        {
            try
            {
                var ventas = await _repositorio.GetVentasConSaldoPendiente();
                var ventasDTO = _mapper.Map<List<VentaDTO>>(ventas);
                return Ok(ventasDTO);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }
        }

        [HttpGet("GetResumenPorFecha/{fecha}")]
        public async Task<ActionResult<ResumenVentasDTO>> GetResumenPorFecha(DateTime fecha)
        {
            try
            {
                var ventasDelDia = await _repositorio.GetByFecha(fecha);
                var ventasCompletadas = ventasDelDia
                    .Where(v => v.Estado == "Pagado" || v.Estado == "Completada")
                    .ToList();

                // Obtener la caja del día
                var caja = await _context.Cajas
                    .FirstOrDefaultAsync(c => c.Fecha.Date == fecha.Date);

                // Obtener egresos del día
                decimal egresos = 0;
                if (caja != null)
                {
                    egresos = await _context.DetallesCaja
                        .Where(d => d.IdCaja == caja.Id && d.Tipo == "EGRESO")
                        .SumAsync(d => d.Monto);
                }

                // Obtener ingresos por medio de pago desde DetallesCaja
                decimal ingresosEfectivo = 0;
                decimal ingresosTransferencia = 0;
                decimal ingresosTarjeta = 0;

                if (caja != null)
                {
                    ingresosEfectivo = await _context.DetallesCaja
                        .Where(d => d.IdCaja == caja.Id && d.Tipo == "INGRESO" && d.Concepto.Contains("Efectivo"))
                        .SumAsync(d => d.Monto);

                    ingresosTransferencia = await _context.DetallesCaja
                        .Where(d => d.IdCaja == caja.Id && d.Tipo == "INGRESO" && d.Concepto.Contains("Transferencia"))
                        .SumAsync(d => d.Monto);

                    ingresosTarjeta = await _context.DetallesCaja
                        .Where(d => d.IdCaja == caja.Id && d.Tipo == "INGRESO" && d.Concepto.Contains("Tarjeta"))
                        .SumAsync(d => d.Monto);
                }

                // Obtener pagos de clientes (deudas)
                var pagosClientesEfectivo = await _context.PagosVenta
                    .Where(p => p.Fecha.Date == fecha.Date && p.MedioPago == "Efectivo")
                    .SumAsync(p => p.Monto);

                var pagosClientesTransferencia = await _context.PagosVenta
                    .Where(p => p.Fecha.Date == fecha.Date && p.MedioPago == "Transferencia")
                    .SumAsync(p => p.Monto);

                var pagosClientesTarjeta = await _context.PagosVenta
                    .Where(p => p.Fecha.Date == fecha.Date && p.MedioPago == "Tarjeta")
                    .SumAsync(p => p.Monto);

                // Obtener pagos a proveedores
                var pagosProveedoresEfectivo = await _context.PagosProveedor
                    .Where(p => p.Fecha.Date == fecha.Date && p.MedioPago == "EFECTIVO")
                    .SumAsync(p => p.Monto);

                var pagosProveedoresTransferencia = await _context.PagosProveedor
                    .Where(p => p.Fecha.Date == fecha.Date && p.MedioPago == "TRANSFERENCIA")
                    .SumAsync(p => p.Monto);

                // Obtener gastos varios
                var gastosEfectivo = await _context.DetallesCaja
                    .Where(d => d.Fecha.Date == fecha.Date && d.Tipo == "EGRESO" && !d.Concepto.Contains("Compra") && d.Concepto.Contains("EFECTIVO"))
                    .SumAsync(d => d.Monto);

                var gastosTransferencia = await _context.DetallesCaja
                    .Where(d => d.Fecha.Date == fecha.Date && d.Tipo == "EGRESO" && !d.Concepto.Contains("Compra") && d.Concepto.Contains("TRANSFERENCIA"))
                    .SumAsync(d => d.Monto);

                // Obtener detalle de egresos
                var egresosProveedores = await _context.PagosProveedor
                    .Include(p => p.Compra)
                        .ThenInclude(c => c.Proveedor)
                    .Where(p => p.Fecha.Date == fecha.Date)
                    .Select(p => new DetalleEgresoDTO
                    {
                        Id = p.Id,
                        Fecha = p.Fecha,
                        Concepto = $"Pago a proveedor: {p.Compra.Proveedor.Nombre}",
                        Monto = p.Monto,
                        MedioPago = p.MedioPago,
                        Tipo = "PROVEEDOR",
                        Referencia = $"Compra #{p.IdCompra}",
                        Proveedor = p.Compra.Proveedor.Nombre
                    }).ToListAsync();

                var egresosGastos = await _context.DetallesCaja
                    .Where(d => d.Fecha.Date == fecha.Date && d.Tipo == "EGRESO" && !d.Concepto.Contains("Compra"))
                    .Select(d => new DetalleEgresoDTO
                    {
                        Id = d.Id,
                        Fecha = d.Fecha,
                        Concepto = d.Concepto,
                        Monto = d.Monto,
                        MedioPago = "EFECTIVO",
                        Tipo = "GASTO",
                        Referencia = d.Referencia
                    }).ToListAsync();

                var detalleEgresos = egresosProveedores.Concat(egresosGastos).OrderByDescending(e => e.Fecha).ToList();

                var resumen = new ResumenVentasDTO
                {
                    // Ventas del día (desde la tabla Ventas)
                    TotalVentas = ventasCompletadas.Sum(v => v.Total),
                    TotalEfectivo = ventasCompletadas.Where(v => v.MetodoPago.Contains("Efectivo")).Sum(v => v.Total),
                    TotalTarjeta = ventasCompletadas.Where(v => v.MetodoPago.Contains("Tarjeta")).Sum(v => v.Total),
                    TotalTransferencia = ventasCompletadas.Where(v => v.MetodoPago.Contains("Transferencia")).Sum(v => v.Total),
                    CantidadVentas = ventasCompletadas.Count,

                    // Pagos de clientes (deudas)
                    PagosClientesEfectivo = pagosClientesEfectivo,
                    PagosClientesTransferencia = pagosClientesTransferencia,
                    PagosClientesTarjeta = pagosClientesTarjeta,

                    // Pagos a proveedores
                    PagosProveedoresEfectivo = pagosProveedoresEfectivo,
                    PagosProveedoresTransferencia = pagosProveedoresTransferencia,

                    // Gastos varios
                    GastosEfectivo = gastosEfectivo,
                    GastosTransferencia = gastosTransferencia,

                    // Egresos totales y detalle
                    TotalEgresos = egresos,
                    DetalleEgresos = detalleEgresos,

                    // Ingresos reales en caja (desde DetallesCaja)
                    TotalIngresosEfectivo = ingresosEfectivo,
                    TotalIngresosTransferencia = ingresosTransferencia,
                    TotalIngresosTarjeta = ingresosTarjeta,

                    // Caja
                    ImporteInicio = caja?.ImporteInicio ?? 0,
                    ImporteCierre = caja?.ImporteCierre
                };

                // Calcular Totales por medio de pago esperados
                resumen.EfectivoEsperado = resumen.ImporteInicio + resumen.TotalIngresosEfectivo - (resumen.PagosProveedoresEfectivo + resumen.GastosEfectivo);
                resumen.TransferenciaEsperada = resumen.TotalIngresosTransferencia - (resumen.PagosProveedoresTransferencia + resumen.GastosTransferencia);
                resumen.TarjetaEsperada = resumen.TotalIngresosTarjeta;
                resumen.TotalEsperado = resumen.EfectivoEsperado + resumen.TransferenciaEsperada + resumen.TarjetaEsperada;
                resumen.Diferencia = (resumen.ImporteCierre ?? 0) - resumen.TotalEsperado;
                resumen.ValidacionMediosPago = Math.Abs(resumen.TotalVentas - (resumen.TotalEfectivo + resumen.TotalTarjeta + resumen.TotalTransferencia)) < 0.01m;

                return Ok(resumen);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }

        [HttpPost]
        public async Task<ActionResult<int>> Post(CrearVentaDTO crearVentaDTO)
        {
            try
            {
                // Validar que existe una caja abierta para hoy
                var cajaHoy = await _context.Cajas
                    .FirstOrDefaultAsync(c => c.Fecha.Date == DateTime.Today.Date && c.Estado == "Abierta");

                if (cajaHoy == null)
                {
                    return BadRequest("❌ No se puede realizar la venta porque no hay una caja abierta para hoy. Por favor, abra una caja primero.");
                }

                // Validar usuario
                var usuario = await _usuarioRepositorio.SelectById(crearVentaDTO.IdUsuario);
                if (usuario == null)
                    return BadRequest("El usuario no existe");

                // Validar stock suficiente
                foreach (var detalle in crearVentaDTO.DetallesVenta)
                {
                    var producto = await _productoRepositorio.SelectByIdWithRelations(detalle.IdProducto);
                    if (producto == null)
                        return BadRequest($"Producto ID {detalle.IdProducto} no existe");

                    if (producto.StockActual < detalle.Cantidad)
                        return BadRequest($"Stock insuficiente para {producto.Descripcion}. Disponible: {producto.StockActual}");
                }

                // Crear venta
                var venta = _mapper.Map<Venta>(crearVentaDTO);
                var idVenta = await _repositorio.Insert(venta);
                if (idVenta <= 0)
                    return BadRequest("No se pudo crear la venta");

                // Procesar detalles y stock
                foreach (var detalle in crearVentaDTO.DetallesVenta)
                {
                    // Insertar detalle
                    var detalleVenta = new DetalleVenta
                    {
                        IdVenta = idVenta,
                        IdProducto = detalle.IdProducto,
                        Cantidad = detalle.Cantidad,
                        PrecioUnitario = detalle.PrecioUnitario
                    };
                    await _detalleVentaRepositorio.Insert(detalleVenta);

                    // Actualizar stock
                    var producto = await _productoRepositorio.SelectByIdWithRelations(detalle.IdProducto);
                    if (producto != null)
                    {
                        int stockAnterior = producto.StockActual;
                        producto.StockActual -= detalle.Cantidad;
                        await _productoRepositorio.Update(producto.Id, producto);

                        // Registrar movimiento de stock
                        var movimiento = new MovimientoStock
                        {
                            IdProducto = detalle.IdProducto,
                            Tipo = "VENTA",
                            Cantidad = detalle.Cantidad,
                            Fecha = DateTime.Now,
                            Referencia = $"Venta #{idVenta}",
                            StockAnterior = stockAnterior,
                            StockNuevo = producto.StockActual
                        };
                        await _movimientoStockRepositorio.Insert(movimiento);
                    }
                }

                // Registrar ingresos en caja por cada medio de pago
                bool hayIngresos = false;

                if (crearVentaDTO.PagoEfectivo > 0)
                {
                    var detalleCajaEfectivo = new DetalleCaja
                    {
                        IdCaja = cajaHoy.Id,
                        Tipo = "INGRESO",
                        Concepto = $"Venta #{idVenta} - Efectivo",
                        Monto = crearVentaDTO.PagoEfectivo,
                        Fecha = DateTime.Now,
                        Referencia = $"Venta #{idVenta}"
                    };
                    _context.DetallesCaja.Add(detalleCajaEfectivo);
                    hayIngresos = true;
                    Console.WriteLine($"✅ Registrado pago en efectivo: {crearVentaDTO.PagoEfectivo:C}");
                }

                if (crearVentaDTO.PagoTransferencia > 0)
                {
                    var detalleCajaTransferencia = new DetalleCaja
                    {
                        IdCaja = cajaHoy.Id,
                        Tipo = "INGRESO",
                        Concepto = $"Venta #{idVenta} - Transferencia",
                        Monto = crearVentaDTO.PagoTransferencia,
                        Fecha = DateTime.Now,
                        Referencia = $"Venta #{idVenta}"
                    };
                    _context.DetallesCaja.Add(detalleCajaTransferencia);
                    hayIngresos = true;
                    Console.WriteLine($"✅ Registrado pago por transferencia: {crearVentaDTO.PagoTransferencia:C}");
                }

                if (crearVentaDTO.PagoTarjeta > 0)
                {
                    var detalleCajaTarjeta = new DetalleCaja
                    {
                        IdCaja = cajaHoy.Id,
                        Tipo = "INGRESO",
                        Concepto = $"Venta #{idVenta} - Tarjeta",
                        Monto = crearVentaDTO.PagoTarjeta,
                        Fecha = DateTime.Now,
                        Referencia = $"Venta #{idVenta}"
                    };
                    _context.DetallesCaja.Add(detalleCajaTarjeta);
                    hayIngresos = true;
                    Console.WriteLine($"✅ Registrado pago con tarjeta: {crearVentaDTO.PagoTarjeta:C}");
                }

                if (hayIngresos)
                {
                    await _context.SaveChangesAsync();
                }

                return Ok(idVenta);
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
                if (id != ventaDTO.Id)
                    return BadRequest("IDs no coinciden");

                var venta = _mapper.Map<Venta>(ventaDTO);
                var resultado = await _repositorio.Update(id, venta);

                if (!resultado)
                    return BadRequest("No se pudo actualizar");

                return Ok();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            try
            {
                var resultado = await _repositorio.Delete(id);
                if (!resultado)
                    return BadRequest("No se pudo eliminar");

                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }
        }
    }
}