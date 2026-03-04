using Blazored.LocalStorage;
using GestionDespensa1.Client;
using GestionDespensa1.Client.Servicios.Auth;
using GestionDespensa1.Client.Servicios.Entidades;
using GestionDespensa1.Client.Servicios.Http;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;


var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// HttpClient base
builder.Services.AddScoped(sp => new HttpClient
{
    BaseAddress = new Uri(builder.HostEnvironment.BaseAddress)
});

builder.Services.AddBlazoredLocalStorage();

builder.Services.AddAuthorizationCore();

builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthStateProvider>();

builder.Services.AddScoped<IAuthService, AuthService>(); 
builder.Services.AddScoped<IVentaService, VentaService>(); 
builder.Services.AddScoped<ICajaService, CajaService>();
builder.Services.AddScoped<IMovimientoStockService, MovimientoStockService>();
builder.Services.AddScoped<IHttpServicio, HttpServicio>();

builder.Services.AddScoped<IClienteService, ClienteService>();
builder.Services.AddScoped<IProductoService, ProductoService>();
builder.Services.AddScoped<ICategoriaService, CategoriaService>();
builder.Services.AddScoped<IProveedorService, ProveedorService>();

builder.Services.AddScoped<IVentaService, VentaService>();
builder.Services.AddScoped<IDetalleVentaService, DetalleVentaService>();

builder.Services.AddScoped<ICompraProveedorService, CompraProveedorService>(); 
builder.Services.AddScoped<IDetalleCompraProveedorService, DetalleCompraProveedorService>();

builder.Services.AddScoped<ICajaService, CajaService>();
builder.Services.AddScoped<IDetalleCajaService, DetalleCajaService>();
builder.Services.AddScoped<IDashboardService, DashboardService>();
builder.Services.AddScoped<IReportesService, ReportesService>();
builder.Services.AddScoped<IPagoProveedorService, PagoProveedorService>();
builder.Services.AddScoped<INotificacionesService, NotificacionesService>();
await builder.Build().RunAsync();