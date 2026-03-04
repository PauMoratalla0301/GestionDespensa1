using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionDespensa1.Shared.DTO
{
    public class LoginRespuestaDTO
    {
        public bool Success { get; set; }
        public string? Token { get; set; }
        public string? Error { get; set; }
        public UsuarioDTO? Usuario { get; set; }
    }
}
