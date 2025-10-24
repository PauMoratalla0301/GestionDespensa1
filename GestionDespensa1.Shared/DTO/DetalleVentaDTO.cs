using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionDespensa1.Shared.DTO
{
    public class DetalleVentaDTO
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El ID de venta es obligatorio.")]
        public int IdVenta { get; set; }

        [Required(ErrorMessage = "El ID de producto es obligatorio.")]
        public int IdProducto { get; set; }

        [Required(ErrorMessage = "La cantidad es obligatoria.")]
        [Range(1, int.MaxValue, ErrorMessage = "La cantidad debe ser mayor a 0.")]
        public int Cantidad { get; set; }

        [Required(ErrorMessage = "El precio unitario es obligatorio.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "El precio unitario debe ser mayor a 0.")]
        public decimal PrecioUnitario { get; set; }

        // Campo calculado
        public decimal Subtotal => Cantidad * PrecioUnitario;

        // Información del producto (para mostrar)
        public string? DescripcionProducto { get; set; }
        public int StockActual { get; set; }
    }
}