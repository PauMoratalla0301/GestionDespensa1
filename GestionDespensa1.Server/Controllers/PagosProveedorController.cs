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
    public class PagosProveedorController : ControllerBase
    {
        private readonly IPagoProveedorRepositorio _repositorio;
        private readonly ICompraProveedorRepositorio _compraRepositorio;
        private readonly IMapper _mapper;
        private readonly Context _context;

        public PagosProveedorController(
            IPagoProveedorRepositorio repositorio,
            ICompraProveedorRepositorio compraRepositorio,
            IMapper mapper,
            Context context)
        {
            _repositorio = repositorio;
            _compraRepositorio = compraRepositorio;
            _mapper = mapper;
            _context = context;
        }

        [HttpGet("GetByCompra/{idCompra:int}")]
        public async Task<ActionResult<List<PagoProveedorDTO>>> GetByCompra(int idCompra)
        {
            try
            {
                var pagos = await _repositorio.GetByCompra(idCompra);
                var pagosDTO = pagos.Select(p => new PagoProveedorDTO
                {
                    Id = p.Id,
                    IdCompra = p.IdCompra,
                    Fecha = p.Fecha,
                    Monto = p.Monto,
                    MedioPago = p.MedioPago,
                    Observaciones = p.Observaciones,
                    Proveedor = p.Compra?.Proveedor?.Nombre,
                    IdProveedor = p.Compra?.IdProveedor,
                    TotalCompra = p.Compra?.Total,
                    PagadoTotal = p.Compra?.DetallesCompra?.Sum(d => d.Cantidad * d.PrecioUnitario)
                }).ToList();

                return Ok(pagosDTO);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }
        }

        [HttpGet("GetByProveedor/{idProveedor:int}")]
        public async Task<ActionResult<List<PagoProveedorDTO>>> GetByProveedor(int idProveedor)
        {
            try
            {
                var pagos = await _repositorio.GetByProveedor(idProveedor);
                var pagosDTO = pagos.Select(p => new PagoProveedorDTO
                {
                    Id = p.Id,
                    IdCompra = p.IdCompra,
                    Fecha = p.Fecha,
                    Monto = p.Monto,
                    MedioPago = p.MedioPago,
                    Observaciones = p.Observaciones,
                    Proveedor = p.Compra?.Proveedor?.Nombre
                }).ToList();

                return Ok(pagosDTO);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }
        }

        [HttpPost]
        public async Task<ActionResult<int>> Post(CrearPagoProveedorDTO crearPagoDTO)
        {
            try
            {
                // Validar compra
                var compra = await _compraRepositorio.SelectById(crearPagoDTO.IdCompra);
                if (compra == null)
                    return BadRequest("Compra no encontrada");

                // Validar que el monto no exceda el total
                var totalPagado = await _repositorio.GetTotalPagadoPorCompra(crearPagoDTO.IdCompra);
                var nuevoTotalPagado = totalPagado + crearPagoDTO.Monto;

                if (nuevoTotalPagado > compra.Total + 0.01m)
                    return BadRequest($"El pago excede el total de la compra. Pendiente: {(compra.Total - totalPagado):C2}");

                // Crear pago
                var pago = new PagoProveedor
                {
                    IdCompra = crearPagoDTO.IdCompra,
                    Fecha = crearPagoDTO.Fecha,
                    Monto = crearPagoDTO.Monto,
                    MedioPago = crearPagoDTO.MedioPago,
                    Observaciones = crearPagoDTO.Observaciones
                };

                var idPago = await _repositorio.Insert(pago);

                // Actualizar estado de la compra
                compra.Estado = nuevoTotalPagado >= compra.Total ? "PAGADA" : "PENDIENTE";
                await _compraRepositorio.Update(compra.Id, compra);

                return Ok(idPago);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }
        }
    }
}