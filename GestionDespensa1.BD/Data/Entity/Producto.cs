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
    [Index(nameof(Descripcion), Name = "Producto_UQ", IsUnique = true)]
    public class Producto : EntityBase
    {
        [Required(ErrorMessage = "La descripción es obligatoria.")]
        [MaxLength(45, ErrorMessage = "Máximo número de caracteres {1}.")]
        [Display(Name = "Descripción")]
        public string Descripcion { get; set; }

        [Required(ErrorMessage = "El precio unitario es obligatorio.")]
        [Display(Name = "Precio Unitario")]
        [Precision(18, 2)]
        public decimal PrecioUnitario { get; set; }

        [Required(ErrorMessage = "El porcentaje de ganancia es obligatorio.")]
        [Display(Name = "Porcentaje Ganancia")]
        [Precision(5, 2)]
        public decimal GananciaPorcentaje { get; set; }

        [Required(ErrorMessage = "El stock actual es obligatorio.")]
        [Display(Name = "Stock Actual")]
        public int StockActual { get; set; }

        [Required(ErrorMessage = "El stock mínimo es obligatorio.")]
        [Display(Name = "Stock Mínimo")]
        public int StockMinimo { get; set; }

        [Required(ErrorMessage = "El ID de categoría es obligatorio.")]
        [Display(Name = "ID Categoría")]
        public int IdCategoria { get; set; }

        // Navigation properties
        [ForeignKey("IdCategoria")]
        public Categoria Categoria { get; set; }
        public List<DetalleVenta> DetallesVenta { get; set; } = new List<DetalleVenta>();
        public List<DetalleCompraProveedor> DetallesCompra { get; set; } = new List<DetalleCompraProveedor>();

    }
}
