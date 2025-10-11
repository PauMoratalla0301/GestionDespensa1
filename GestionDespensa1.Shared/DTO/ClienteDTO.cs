using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionDespensa1.Shared.DTO
{
    public class ClienteDTO
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Apellido { get; set; } = string.Empty;
        public string Dni { get; set; } = string.Empty;
        public string? Telefono { get; set; }
        public string? Direccion { get; set; }
        public string? Email { get; set; } // ← AGREGAR ESTA PROPIEDAD
        public decimal SaldoPendiente { get; set; }
        public List<VentaDTO> Ventas { get; set; } = new List<VentaDTO>();
    }
}
