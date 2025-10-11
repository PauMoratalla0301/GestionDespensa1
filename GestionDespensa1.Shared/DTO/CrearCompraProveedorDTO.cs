using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionDespensa1.Shared.DTO
{
    public class CrearCompraProveedorDTO
    {
        [Required(ErrorMessage = "El ID de proveedor es obligatorio.")]
        public int IdProveedor { get; set; }

        [Required(ErrorMessage = "La fecha de compra es obligatoria.")]
        [MaxLength(45, ErrorMessage = "Máximo número de caracteres {1}.")]
        public string FechaCompra { get; set; } = string.Empty;

        [MaxLength(45, ErrorMessage = "Máximo número de caracteres {1}.")]
        public string Observaciones { get; set; } = string.Empty;
    }
}
