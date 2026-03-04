using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GestionDespensa1.BD.Data.Entity
{
    public class MovimientoStock : EntityBase
    {
        [Required(ErrorMessage = "El producto es obligatorio.")]
        public int IdProducto { get; set; }

        [Required(ErrorMessage = "El tipo de movimiento es obligatorio.")]
        [MaxLength(20, ErrorMessage = "Máximo {1} caracteres.")]
        public string Tipo { get; set; }  // VENTA, COMPRA, AJUSTE

        [Required(ErrorMessage = "La cantidad es obligatoria.")]
        public int Cantidad { get; set; }

        [Required(ErrorMessage = "La fecha es obligatoria.")]
        public DateTime Fecha { get; set; }

        [MaxLength(500, ErrorMessage = "Máximo {1} caracteres.")]
        public string? Referencia { get; set; }  // Ej: "Venta #123", "Compra #45", "Ajuste manual"

        [MaxLength(500, ErrorMessage = "Máximo {1} caracteres.")]
        public string? Observaciones { get; set; }

        // Stock resultante después del movimiento (opcional pero útil)
        public int StockAnterior { get; set; }
        public int StockNuevo { get; set; }

        // Navigation property
        [ForeignKey("IdProducto")]
        public Producto Producto { get; set; }
    }
}