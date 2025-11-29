using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionDespensa1.Shared.DTO
{
    public class ResumenVentasDTO
    {
        public decimal TotalVentas { get; set; }
        public decimal TotalEfectivo { get; set; }
        public decimal TotalTarjeta { get; set; }
        public decimal TotalTransferencia { get; set; }
        public int CantidadVentas { get; set; }
        //propiedades para agregar
        public decimal TotalEgresos { get; set; }
        public decimal ImporteInicio { get; set; }
        public decimal? ImporteCierre { get; set; }

        // Propiedades calculadas
        public decimal TotalIngresos => ImporteInicio + TotalVentas;
        public decimal EfectivoEsperado => ImporteInicio + TotalEfectivo - TotalEgresos;
        public decimal Diferencia => (ImporteCierre ?? 0) - EfectivoEsperado;
        // Validación de medios de pago
        public bool ValidacionMediosPago =>
            Math.Abs(TotalVentas - (TotalEfectivo + TotalTarjeta + TotalTransferencia)) < 0.01m;
    }
}