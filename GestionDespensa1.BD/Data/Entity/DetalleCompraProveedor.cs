using GestionDespensa1.BD.Data;
using GestionDespensa1.BD.Data.Entity;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

public class DetalleCompraProveedor : EntityBase
{
    public int IdCompra { get; set; }
    public int IdProducto { get; set; }
    public int Cantidad { get; set; }
    [Precision(18, 2)] 
    public decimal PrecioUnitario { get; set; }
    public decimal Subtotal => Cantidad * PrecioUnitario;

    // Navigation properties
    [ForeignKey("IdCompra")]
    public CompraProveedor CompraProveedor { get; set; }

    [ForeignKey("IdProducto")]
    public Producto Producto { get; set; }
}