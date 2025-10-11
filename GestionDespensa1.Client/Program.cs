using GestionDespensa1.Client;
using GestionDespensa1.Client.Servicios.Entidades;
using GestionDespensa1.Client.Servicios.Http;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient
{
    BaseAddress = new Uri(builder.HostEnvironment.BaseAddress)
});

// ===== REGISTRO DE SERVICIOS HTTP =====

// Servicios HTTP base
builder.Services.AddScoped<IHttpServicio, HttpServicio>();

// Servicios de entidades principales
builder.Services.AddScoped<IClienteService, ClienteService>();
builder.Services.AddScoped<IProductoService, ProductoService>();
builder.Services.AddScoped<ICategoriaService, CategoriaService>();
builder.Services.AddScoped<IProveedorService, ProveedorService>();

// Servicios de ventas
builder.Services.AddScoped<IVentaService, VentaService>();
builder.Services.AddScoped<IDetalleVentaService, DetalleVentaService>();

// Servicios de compras a proveedores
builder.Services.AddScoped<ICompraProveedorService, CompraProveedorService>();
builder.Services.AddScoped<IDetalleCompraProveedorService, DetalleCompraProveedorService>();

// Servicios de caja
builder.Services.AddScoped<ICajaService, CajaService>();
builder.Services.AddScoped<IDetalleCajaService, DetalleCajaService>();

await builder.Build().RunAsync();