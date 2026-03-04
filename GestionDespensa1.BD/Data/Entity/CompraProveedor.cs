using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GestionDespensa1.BD.Data.Entity
{
    public class CompraProveedor : EntityBase
    {
        [Required]
        public int IdProveedor { get; set; }

        [Required]
        public DateTime FechaCompra { get; set; }

        [Required]
        [Precision(18, 2)]
        public decimal Total { get; set; }

        [Required]
        [MaxLength(20)]
        public string Estado { get; set; } = "PENDIENTE";

        [MaxLength(50)]
        public string? MetodoPago { get; set; } = "EFECTIVO";  // 👈 NUEVO

        [MaxLength(500)]
        public string? Observaciones { get; set; }

        // Navigation properties
        [ForeignKey("IdProveedor")]
        public Proveedor Proveedor { get; set; }
        public List<DetalleCompraProveedor> DetallesCompra { get; set; } = new();
    }
}