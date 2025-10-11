using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionDespensa1.BD.Data.Entity
{
    public class DetalleCompraProveedor : EntityBase
    {
        [Required(ErrorMessage = "El ID de compra es obligatorio.")]
        [Display(Name = "ID Compra")]
        public int IdCompra { get; set; }

        [Required(ErrorMessage = "El ID de producto es obligatorio.")]
        [Display(Name = "ID Producto")]
        public int IdProducto { get; set; }

        [Required(ErrorMessage = "La cantidad es obligatoria.")]
        [Display(Name = "Cantidad")]
        public int Cantidad { get; set; }

        [Required(ErrorMessage = "El precio unitario es obligatorio.")]
        [Display(Name = "Precio Unitario")]
        public decimal PrecioUnitario { get; set; } // Cambiado de int a decimal

        // Navigation properties
        [ForeignKey("IdCompra")]
        public CompraProveedor CompraProveedor { get; set; }

        [ForeignKey("IdProducto")]
        public Producto Producto { get; set; }
    }
}
