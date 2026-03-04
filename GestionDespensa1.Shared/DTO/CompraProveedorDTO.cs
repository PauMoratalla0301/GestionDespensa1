using GestionDespensa1.Shared.DTO;

public class CompraProveedorDTO
{
    public int Id { get; set; }
    public int IdProveedor { get; set; }
    public string? NombreProveedor { get; set; }
    public DateTime FechaCompra { get; set; }
    public decimal Total { get; set; }
    public decimal? PagadoTotal { get; set; }
    public string Estado { get; set; } = "PENDIENTE";
    public string MetodoPago { get; set; } = "EFECTIVO"; // 👈 NUEVO
    public string? Observaciones { get; set; }
    public List<DetalleCompraProveedorDTO> DetallesCompra { get; set; } = new();
}