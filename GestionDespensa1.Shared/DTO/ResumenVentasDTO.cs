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
        public decimal TotalIngresos => ImporteInicio + TotalVentas;
        public decimal Diferencia { get; set; }
        public decimal ImporteInicio { get; set; }
        public decimal? ImporteCierre { get; set; }
    }
}