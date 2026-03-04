namespace GestionDespensa1.Shared.DTO
{
    public class ClienteFidelizacionDTO
    {
        public int IdCliente { get; set; }
        public string NombreCliente { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Telefono { get; set; } = string.Empty;
        public DateTime? FechaNacimiento { get; set; }
        public int PuntosAcumulados { get; set; }
        public decimal TotalCompras { get; set; }
        public int CantidadCompras { get; set; }
        public DateTime UltimaCompra { get; set; }
        public string Categoria { get; set; } = "Ocasional"; // VIP, Frecuente, Ocasional, Nuevo
        public List<CompraResumenDTO> UltimasCompras { get; set; } = new();
    }

    public class CompraResumenDTO
    {
        public int IdVenta { get; set; }
        public DateTime Fecha { get; set; }
        public decimal Total { get; set; }
        public int PuntosGanados { get; set; }
    }

    public class PromocionDTO
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
        public string Tipo { get; set; } = string.Empty; // "PORCENTAJE", "PUNTOS", "PRODUCTO"
        public decimal Valor { get; set; }
        public int? PuntosRequeridos { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public bool Activa { get; set; }
    }
}