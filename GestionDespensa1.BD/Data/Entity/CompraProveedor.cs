using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionDespensa1.BD.Data.Entity
{
    public class CompraProveedor : EntityBase
    {
        [Required(ErrorMessage = "El ID de proveedor es obligatorio.")]
        [Display(Name = "ID Proveedor")]
        public int IdProveedor { get; set; }

        [Required(ErrorMessage = "La fecha de compra es obligatoria.")]
        [MaxLength(45, ErrorMessage = "Máximo número de caracteres {1}.")]
        [Display(Name = "Fecha Compra")]
        public string FechaCompra { get; set; }

        [MaxLength(45, ErrorMessage = "Máximo número de caracteres {1}.")]
        [Display(Name = "Observaciones")]
        public string Observaciones { get; set; }

        // Navigation properties
        [ForeignKey("IdProveedor")]
        public Proveedor Proveedor { get; set; }
        public List<DetalleCompraProveedor> DetallesCompra { get; set; } = new List<DetalleCompraProveedor>();



    }
}
