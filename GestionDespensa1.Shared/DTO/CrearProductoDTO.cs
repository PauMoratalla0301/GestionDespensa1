using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionDespensa1.Shared.DTO
{
    public class CrearProductoDTO
    {
        [Required(ErrorMessage = "La descripción es obligatoria.")]
        [MaxLength(45, ErrorMessage = "Máximo número de caracteres {1}.")]
        public string Descripcion { get; set; }

        [Required(ErrorMessage = "El precio unitario es obligatorio.")]
        public decimal PrecioUnitario { get; set; }

        [Required(ErrorMessage = "El porcentaje de ganancia es obligatorio.")]
        public decimal GananciaPorcentaje { get; set; }

        [Required(ErrorMessage = "El stock actual es obligatorio.")]
        public int StockActual { get; set; }

        [Required(ErrorMessage = "El stock mínimo es obligatorio.")]
        public int StockMinimo { get; set; }

        [Required(ErrorMessage = "El ID de categoría es obligatorio.")]
        public int IdCategoria { get; set; }
    }
}
