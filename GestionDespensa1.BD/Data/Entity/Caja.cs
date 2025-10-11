using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionDespensa1.BD.Data.Entity
{
    public class Caja : EntityBase
    {

        [Required(ErrorMessage = "El ID de usuario es obligatorio.")]
        [MaxLength(45, ErrorMessage = "Máximo número de caracteres {1}.")]
        [Display(Name = "ID Usuario")]
        public string IdUsuario { get; set; }

        [Required(ErrorMessage = "La fecha es obligatoria.")]
        [Display(Name = "Fecha")]
        public DateTime Fecha { get; set; }

        [Required(ErrorMessage = "El importe de inicio es obligatorio.")]
        [Display(Name = "Importe Inicio")]
        public decimal ImporteInicio { get; set; }

        // Navigation properties
        public List<DetalleCaja> DetallesCaja { get; set; } = new List<DetalleCaja>();



    }
}
