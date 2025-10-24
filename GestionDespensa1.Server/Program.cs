using GestionDespensa1.BD.Data;
using GestionDespensa1.Server.Repositorio;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Servicios
builder.Services.AddControllers().AddJsonOptions(
    x => x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// ✅ CONEXIÓN SIMPLIFICADA - Siempre usa Somee en el servidor
builder.Services.AddDbContext<Context>(op =>
    op.UseSqlServer(builder.Configuration.GetConnectionString("conn2")));

builder.Services.AddAutoMapper(typeof(Program));

// Tus repositorios...
builder.Services.AddScoped<ICajaRepositorio, CajaRepositorio>();
builder.Services.AddScoped<IClienteRepositorio, ClienteRepositorio>();
builder.Services.AddScoped<IProductoRepositorio, ProductoRepositorio>();
builder.Services.AddScoped<IVentaRepositorio, VentaRepositorio>();
builder.Services.AddScoped<ICategoriaRepositorio, CategoriaRepositorio>();
builder.Services.AddScoped<IProveedorRepositorio, ProveedorRepositorio>();
builder.Services.AddScoped<IDetalleCajaRepositorio, DetalleCajaRepositorio>();
builder.Services.AddScoped<IDetalleVentaRepositorio, DetalleVentaRepositorio>();
builder.Services.AddScoped<ICompraProveedorRepositorio, CompraProveedorRepositorio>();
builder.Services.AddScoped<IDetalleCompraProveedorRepositorio, DetalleCompraProveedorRepositorio>();

var app = builder.Build();

// ✅ SOLO verificación básica - SIN migraciones automáticas
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<Context>();

        if (context.Database.CanConnect())
        {
            Console.WriteLine("✅ Conexión exitosa con la base de datos Somee!");
        }
        else
        {
            Console.WriteLine("⚠️ No se pudo conectar a la base de datos");
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine("❌ Error de conexión: " + ex.Message);
    }
}

// Configuración del pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseBlazorFrameworkFiles();
app.UseStaticFiles();
app.UseRouting();
app.MapRazorPages();
app.UseAuthorization();
app.MapControllers();
app.MapFallbackToFile("index.html");

app.Run();