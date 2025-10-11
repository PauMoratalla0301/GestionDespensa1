using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionDespensa1.BD.Data.Entity
{
    public class DetalleVenta : EntityBase
    {
        [Required(ErrorMessage = "El ID de venta es obligatorio.")]
        [Display(Name = "ID Venta")]
        public int IdVenta { get; set; }

        [Required(ErrorMessage = "El ID de producto es obligatorio.")]
        [Display(Name = "ID Producto")]
        public int IdProducto { get; set; }

        [Required(ErrorMessage = "La cantidad es obligatoria.")]
        [Display(Name = "Cantidad")]
        public int Cantidad { get; set; }

        [Required(ErrorMessage = "El precio unitario es obligatorio.")]
        [Display(Name = "Precio Unitario")]
        [Precision(18, 2)]
        public decimal PrecioUnitario { get; set; }

        // Navigation properties
        [ForeignKey("IdVenta")]
        public Venta Venta { get; set; }

        [ForeignKey("IdProducto")]
        public Producto Producto { get; set; }




    }
}
