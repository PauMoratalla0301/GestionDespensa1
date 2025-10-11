using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionDespensa1.Shared.DTO
{
    public class CrearDetalleCajaDTO
    {
        [Required(ErrorMessage = "El ID de venta es obligatorio.")]
        [MaxLength(45, ErrorMessage = "Máximo número de caracteres {1}.")]
        public string IdVenta { get; set; }

        [Required(ErrorMessage = "El monto es obligatorio.")]
        [MaxLength(45, ErrorMessage = "Máximo número de caracteres {1}.")]
        public string Monto { get; set; }

        [Required(ErrorMessage = "El ID de caja es obligatorio.")]
        public int IdCaja { get; set; }
    }
}
