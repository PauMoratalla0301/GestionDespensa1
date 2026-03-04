using System;
using System.Collections.Generic;

namespace GestionDespensa1.Shared.DTO
{
    // Reporte de Ventas por Período
    public class ReporteVentasDTO
    {
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public int TotalVentas { get; set; }
        public decimal TotalIngresos { get; set; }
        public decimal TotalEfectivo { get; set; }
        public decimal TotalTarjeta { get; set; }
        public decimal TotalTransferencia { get; set; }
        public List<VentaResumenDTO> Ventas { get; set; } = new();
    }

    public class VentaResumenDTO
    {
        public int Id { get; set; }
        public DateTime Fecha { get; set; }
        public string Cliente { get; set; } = string.Empty;
        public decimal Total { get; set; }
        public string Estado { get; set; } = string.Empty;
        public string MetodoPago { get; set; } = string.Empty;
    }

    // Productos más vendidos
    public class ReporteProductosDTO
    {
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public List<ProductoVendidoDTO> Productos { get; set; } = new();
    }

    public class ProductoVendidoDTO
    {
        public int IdProducto { get; set; }
        public string Producto { get; set; } = string.Empty;
        public string Categoria { get; set; } = string.Empty;
        public int CantidadVendida { get; set; }
        public decimal TotalVendido { get; set; }
    }

    // Clientes deudores
    public class ReporteDeudoresDTO
    {
        public List<ClienteDeudorDTO> Deudores { get; set; } = new();
        public decimal TotalGeneral { get; set; }
        public int CantidadDeudores { get; set; }
    }

    public class ClienteDeudorDTO
    {
        public int IdCliente { get; set; }
        public string Cliente { get; set; } = string.Empty;
        public string Telefono { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public decimal SaldoPendiente { get; set; }
        public List<VentaDeudorDTO> Ventas { get; set; } = new();
    }

    public class VentaDeudorDTO
    {
        public int IdVenta { get; set; }
        public DateTime Fecha { get; set; }
        public decimal Total { get; set; }
        public decimal Pagado { get; set; }
        public decimal Saldo { get; set; }
    }

    // Stock bajo
    public class ReporteStockBajoDTO
    {
        public List<ProductoStockBajoDTO> Productos { get; set; } = new();
        public int TotalProductos { get; set; }
    }

    public class ProductoStockBajoDTO
    {
        public int IdProducto { get; set; }
        public string Producto { get; set; } = string.Empty;
        public string Categoria { get; set; } = string.Empty;
        public int StockActual { get; set; }
        public int StockMinimo { get; set; }
        public int Faltante => StockMinimo - StockActual;
    }
}