using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GestionDespensa1.BD.Data.Entity
{
    public class DetalleCaja : EntityBase
    {
        [Required]
        public int IdCaja { get; set; }

        [Required]
        [MaxLength(20)]
        public string Tipo { get; set; } = "INGRESO"; // INGRESO / EGRESO

        [Required]
        [MaxLength(200)]
        public string Concepto { get; set; } = string.Empty;

        [Required]
        [Precision(18, 2)]
        public decimal Monto { get; set; }

        public DateTime Fecha { get; set; } = DateTime.Now;

        public string? Referencia { get; set; } // Ej: "Venta #123", "Compra #45"

        // Navigation property
        [ForeignKey("IdCaja")]
        public Caja Caja { get; set; }
    }
}