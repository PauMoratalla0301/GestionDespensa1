using System;
using System.Collections.Generic;

namespace GestionDespensa1.Shared.DTO
{
    public class ResumenVentasDTO
    {
        // Ventas del día
        public decimal TotalVentas { get; set; }
        public int CantidadVentas { get; set; }

        // Desglose por medio de pago (ventas)
        public decimal TotalEfectivo { get; set; }
        public decimal TotalTarjeta { get; set; }
        public decimal TotalTransferencia { get; set; }
        public decimal TotalOtros { get; set; }

        // Pagos recibidos de clientes (cuando pagan deudas anteriores)
        public decimal PagosClientesEfectivo { get; set; }
        public decimal PagosClientesTransferencia { get; set; }
        public decimal PagosClientesTarjeta { get; set; }

        // Egresos (gastos y pagos a proveedores)
        public decimal TotalEgresos { get; set; }
        public List<DetalleEgresoDTO> DetalleEgresos { get; set; } = new();

        // Pagos a proveedores (desglosado)
        public decimal PagosProveedoresEfectivo { get; set; }
        public decimal PagosProveedoresTransferencia { get; set; }

        // Gastos varios
        public decimal GastosEfectivo { get; set; }
        public decimal GastosTransferencia { get; set; }

        // Totales generales por medio de pago (lo que realmente ingresó)
        public decimal TotalIngresosEfectivo { get; set; }
        public decimal TotalIngresosTransferencia { get; set; }
        public decimal TotalIngresosTarjeta { get; set; }

        // Caja
        public decimal ImporteInicio { get; set; }
        public decimal? ImporteCierre { get; set; }

        // Propiedades calculadas (AHORA EDITABLES)
        public decimal TotalIngresos { get; set; }
        public decimal TotalEgresosCalculado { get; set; }
        public decimal EfectivoEsperado { get; set; }
        public decimal TransferenciaEsperada { get; set; }
        public decimal TarjetaEsperada { get; set; }
        public decimal TotalEsperado { get; set; }
        public decimal Diferencia { get; set; }
        public bool ValidacionMediosPago { get; set; }
    }

    public class DetalleEgresoDTO
    {
        public int Id { get; set; }
        public DateTime Fecha { get; set; }
        public string Concepto { get; set; } = string.Empty;
        public decimal Monto { get; set; }
        public string MedioPago { get; set; } = string.Empty;
        public string Tipo { get; set; } = string.Empty; // "PROVEEDOR" o "GASTO"
        public string? Referencia { get; set; }
        public string? Proveedor { get; set; }
    }
}