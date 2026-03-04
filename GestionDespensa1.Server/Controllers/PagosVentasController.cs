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
    [Route("api/[controller]")]
    public class PagosVentaController : ControllerBase
    {
        private readonly IPagoVentaRepositorio _repositorio;
        private readonly IVentaRepositorio _ventaRepositorio;
        private readonly IMapper _mapper;
        private readonly Context _context;

        public PagosVentaController(
            IPagoVentaRepositorio repositorio,
            IVentaRepositorio ventaRepositorio,
            IMapper mapper,
            Context context)
        {
            _repositorio = repositorio;
            _ventaRepositorio = ventaRepositorio;
            _mapper = mapper;
            _context = context;
        }

        [HttpGet("GetByVenta/{idVenta:int}")]
        public async Task<ActionResult<List<PagoVentaDTO>>> GetByVenta(int idVenta)
        {
            try
            {
                var pagos = await _repositorio.GetByVenta(idVenta);
                var pagosDTO = pagos.Select(p => new PagoVentaDTO
                {
                    Id = p.Id,
                    IdVenta = p.IdVenta,
                    Fecha = p.Fecha,
                    Monto = p.Monto,
                    MedioPago = p.MedioPago,
                    Observaciones = p.Observaciones,
                    Cliente = p.Venta?.Cliente != null ?
                        p.Venta.Cliente.Nombre + " " + p.Venta.Cliente.Apellido : ""
                }).ToList();

                return Ok(pagosDTO);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }
        }

        [HttpGet("GetByCliente/{idCliente:int}")]
        public async Task<ActionResult<List<PagoVentaDTO>>> GetByCliente(int idCliente)
        {
            try
            {
                var pagos = await _repositorio.GetByCliente(idCliente);
                var pagosDTO = pagos.Select(p => new PagoVentaDTO
                {
                    Id = p.Id,
                    IdVenta = p.IdVenta,
                    Fecha = p.Fecha,
                    Monto = p.Monto,
                    MedioPago = p.MedioPago,
                    Observaciones = p.Observaciones
                }).ToList();

                return Ok(pagosDTO);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }
        }

        [HttpPost]
        public async Task<ActionResult<int>> Post(CrearPagoVentaDTO crearPagoDTO)
        {
            try
            {
                // Validar venta
                var venta = await _ventaRepositorio.SelectById(crearPagoDTO.IdVenta);
                if (venta == null)
                    return BadRequest("Venta no encontrada");

                // Validar que el monto no exceda el saldo pendiente
                var totalPagado = await _repositorio.GetTotalPagadoPorVenta(crearPagoDTO.IdVenta);
                var nuevoTotalPagado = totalPagado + crearPagoDTO.Monto;

                if (nuevoTotalPagado > venta.Total + 0.01m) // Tolerancia por redondeo
                    return BadRequest($"El pago excede el total de la venta. Pendiente: {(venta.Total - totalPagado):C2}");

                // Crear pago
                var pago = new PagoVenta
                {
                    IdVenta = crearPagoDTO.IdVenta,
                    Fecha = crearPagoDTO.Fecha,
                    Monto = crearPagoDTO.Monto,
                    MedioPago = crearPagoDTO.MedioPago,
                    Observaciones = crearPagoDTO.Observaciones
                };

                var idPago = await _repositorio.Insert(pago);

                // Actualizar venta
                venta.MontoPagado = nuevoTotalPagado;
                venta.SaldoPendiente = venta.Total - nuevoTotalPagado;
                venta.Estado = venta.SaldoPendiente <= 0 ? "Pagado" : "Parcial";

                await _ventaRepositorio.Update(venta.Id, venta);

                return Ok(idPago);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }
        }
    }
}