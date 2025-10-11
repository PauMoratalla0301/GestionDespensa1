using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionDespensa1.BD.Data.Entity
{
    public class DetalleCaja : EntityBase
    {
        [Required(ErrorMessage = "El ID de venta es obligatorio.")]
        [MaxLength(45, ErrorMessage = "Máximo número de caracteres {1}.")]
        [Display(Name = "ID Venta")]
        public string IdVenta { get; set; }

        [Required(ErrorMessage = "El monto es obligatorio.")]
        [MaxLength(45, ErrorMessage = "Máximo número de caracteres {1}.")]
        [Display(Name = "Monto")]
        public string Monto { get; set; }

        [Required(ErrorMessage = "El ID de caja es obligatorio.")]
        [Display(Name = "ID Caja")]
        public int IdCaja { get; set; }

        // Navigation properties
        [ForeignKey("IdCaja")]
        public Caja Caja { get; set; }


    }
}
