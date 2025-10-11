using System.ComponentModel.DataAnnotations;

namespace GestionDespensa1.Shared.DTO
{
    public class ProductoDTO
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "La descripción es obligatoria.")]
        [MaxLength(45, ErrorMessage = "Máximo número de caracteres {1}.")]
        public string Descripcion { get; set; } = string.Empty;

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

        // AGREGAR ESTA PROPIEDAD
        public CategoriaDTO? Categoria { get; set; }
    }
}