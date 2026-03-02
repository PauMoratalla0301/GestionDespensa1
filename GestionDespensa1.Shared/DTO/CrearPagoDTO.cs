using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionDespensa1.Shared.DTO
{
    public class CrearPagoDTO
    {
        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal MontoTotal { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal MontoPagado { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal SaldoPendiente { get; set; }

        [Required]
        public DateTime FechaPago { get; set; } = DateTime.Now;

        // FK : venta
        [Required]
        public int IdVenta { get; set; }

        [Required]
        public int IdTipoPago { get; set; }

    }    
}
