using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GestionDespensa1.BD.Data.Entity
{
    public class PagoProveedor : EntityBase
    {
        [Required]
        public int IdCompra { get; set; }

        [Required]
        public DateTime Fecha { get; set; }

        [Required]
        [Precision(18, 2)]
        public decimal Monto { get; set; }

        [Required]
        [MaxLength(50)]
        public string MedioPago { get; set; } = "Efectivo"; // EFECTIVO, TRANSFERENCIA, CHEQUE

        [MaxLength(500)]
        public string? Observaciones { get; set; }

        // Navigation property
        [ForeignKey("IdCompra")]
        public CompraProveedor Compra { get; set; }
    }
}