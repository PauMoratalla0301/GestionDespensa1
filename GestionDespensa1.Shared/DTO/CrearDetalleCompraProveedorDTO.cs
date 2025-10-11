using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionDespensa1.Shared.DTO
{
    public class CrearDetalleCompraProveedorDTO
    {
        [Required(ErrorMessage = "El ID de compra es obligatorio.")]
        public int IdCompra { get; set; }

        [Required(ErrorMessage = "El ID de producto es obligatorio.")]
        public int IdProducto { get; set; }

        [Required(ErrorMessage = "La cantidad es obligatoria.")]
        public int Cantidad { get; set; }

        [Required(ErrorMessage = "El precio unitario es obligatorio.")]
        public decimal PrecioUnitario { get; set; }
    }
}
