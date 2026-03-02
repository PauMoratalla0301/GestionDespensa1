using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionDespensa1.BD.Data.Entity
{
    public class Pago : EntityBase
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
        [Display(Name ="Venta")]
        public int IdVenta { get; set; }

        [ForeignKey("IdVenta")]
        public Venta Venta { get; set; }
        // FK:TipoPago (efectivo,tarjeta, transferencia
        [Required]
        [Display(Name = "Tipo de Pago")]
        public int IdTipoPago { get; set; }

        [ForeignKey("IdTipoPago")]
        public TipoPago TipoPago { get; set; }
    }
}
