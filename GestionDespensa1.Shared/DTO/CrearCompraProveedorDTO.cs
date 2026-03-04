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
        public DateTime FechaCompra { get; set; }

        [Required(ErrorMessage = "El estado es obligatorio.")]
        [MaxLength(20, ErrorMessage = "Máximo número de caracteres {1}.")]
        public string Estado { get; set; } = "PENDIENTE";

        [MaxLength(50, ErrorMessage = "Máximo número de caracteres {1}.")]
        public string MetodoPago { get; set; } = "EFECTIVO";

        [MaxLength(500, ErrorMessage = "Máximo número de caracteres {1}.")]
        public string? Observaciones { get; set; }

        // 👇 NUEVO: ID del usuario que registra la compra
        [Required(ErrorMessage = "El ID de usuario es obligatorio.")]
        public int IdUsuario { get; set; }

        public List<DetalleCompraProveedorDTO> DetallesCompra { get; set; } = new List<DetalleCompraProveedorDTO>();
    }
}