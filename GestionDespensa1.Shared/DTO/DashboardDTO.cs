using System;
using System.Collections.Generic;

namespace GestionDespensa1.Shared.DTO
{
    public class DashboardDTO
    {
        // Resumen de Ventas
        public decimal VentasHoy { get; set; }
        public decimal VentasMes { get; set; }
        public int CantidadVentasHoy { get; set; }
        public int CantidadVentasMes { get; set; }

        // Resumen de Stock
        public int ProductosStockBajo { get; set; }
        public int TotalProductos { get; set; }

        // Resumen de Caja
        public bool CajaAbierta { get; set; }
        public decimal? CajaInicio { get; set; }
        public decimal? CajaActual { get; set; }

        // Cuentas por cobrar/pagar
        public decimal TotalClientesDeudores { get; set; }
        public int CantidadClientesDeudores { get; set; }
        public decimal TotalComprasPendientes { get; set; }

        // Últimos movimientos
        public List<UltimaVentaDTO> UltimasVentas { get; set; } = new();
        public List<UltimoMovimientoStockDTO> UltimosMovimientosStock { get; set; } = new();

        // Productos más vendidos
        public List<ProductoMasVendidoDTO> ProductosMasVendidos { get; set; } = new();

        // Ventas por día de la semana (para gráfico)
        public List<VentaDiariaDTO> VentasUltimos7Dias { get; set; } = new();
    }

    // 👇 TODAS ESTAS CLASES VAN DENTRO DEL MISMO ARCHIVO
    public class UltimaVentaDTO
    {
        public int Id { get; set; }
        public DateTime Fecha { get; set; }
        public string Cliente { get; set; } = string.Empty;
        public decimal Total { get; set; }
        public string Estado { get; set; } = string.Empty;
    }

    public class UltimoMovimientoStockDTO
    {
        public int Id { get; set; }
        public string Producto { get; set; } = string.Empty;
        public string Tipo { get; set; } = string.Empty;
        public int Cantidad { get; set; }
        public DateTime Fecha { get; set; }
        public string Referencia { get; set; } = string.Empty;
    }

    public class ProductoMasVendidoDTO
    {
        public string Producto { get; set; } = string.Empty;
        public int CantidadVendida { get; set; }
        public decimal TotalVendido { get; set; }
    }

    public class VentaDiariaDTO
    {
        public DateTime Dia { get; set; }
        public string DiaSemana { get; set; } = string.Empty;
        public decimal Total { get; set; }
        public int Cantidad { get; set; }
    }
}