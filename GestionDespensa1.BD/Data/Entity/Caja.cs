using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GestionDespensa1.BD.Data.Entity
{
    public class Caja : EntityBase
    {
        [Required(ErrorMessage = "El ID de usuario es obligatorio.")]
        public int IdUsuario { get; set; }  // 👈 CAMBIADO DE STRING A INT

        [Required(ErrorMessage = "La fecha es obligatoria.")]
        public DateTime Fecha { get; set; }

        [Required(ErrorMessage = "El importe de inicio es obligatorio.")]
        [Precision(18, 2)]
        public decimal ImporteInicio { get; set; }

        [Precision(18, 2)]
        public decimal? ImporteCierre { get; set; }

        [Required]
        [MaxLength(20)]
        public string Estado { get; set; } = "Abierta";

        [MaxLength(500)]
        public string? Observaciones { get; set; }

        // Navigation properties
        [ForeignKey("IdUsuario")]
        public Usuario Usuario { get; set; }

        public List<DetalleCaja> DetallesCaja { get; set; } = new List<DetalleCaja>();
    }
}