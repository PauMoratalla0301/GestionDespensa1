using GestionDespensa1.BD.Data;
using GestionDespensa1.Server.Repositorio;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// ✅ CONFIGURACIÓN BÁSICA DE SERVICIOS
builder.Services.AddControllers().AddJsonOptions(
    x => x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// ✅ CONFIGURACIÓN DE BD - VERSIÓN SIMPLIFICADA
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

if (string.IsNullOrEmpty(connectionString))
{
    // Fallback si no encuentra la conexión
    if (builder.Environment.IsDevelopment())
    {
        connectionString = "Server=(localdb)\\MSSQLLocalDB;Database=GestionDespensaLocal;Trusted_Connection=True;TrustServerCertificate=True";
    }
    else
    {
        connectionString = "workstation id=DespensaRaquelBD.mssql.somee.com;packet size=4096;user id=PaulaMoratalla_SQLLogin_1;pwd=xfapk6pw6i;data source=DespensaRaquelBD.mssql.somee.com;persist security info=False;initial catalog=DespensaRaquelBD;TrustServerCertificate=True";
    }
}

Console.WriteLine($"🔗 Usando conexión: {connectionString}");

// ✅ CONFIGURACIÓN DE ENTITY FRAMEWORK
builder.Services.AddDbContext<Context>(options =>
    options.UseSqlServer(connectionString));

// ✅ CONFIGURACIÓN AUTOMAPPER SEGURA
builder.Services.AddAutoMapper(typeof(Program).Assembly);

// ✅ TUS REPOSITORIOS
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

// ✅ CONFIGURACIÓN DEL PIPELINE - VERSIÓN SIMPLIFICADA

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseDeveloperExceptionPage();

    Console.WriteLine("🚀 MODO DESARROLLO - BD Local");

    // ✅ INICIALIZACIÓN BD SOLO EN DESARROLLO
    try
    {
        using var scope = app.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<Context>();

        Console.WriteLine("🔄 Creando BD local...");
        await context.Database.EnsureCreatedAsync();
        Console.WriteLine("✅ BD local lista");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"⚠️ Error con BD local: {ex.Message}");
        // No detenemos la aplicación por esto
    }
}
else
{
    Console.WriteLine("🚀 MODO PRODUCCIÓN - Somee");

    // ✅ SOLO VERIFICAR CONEXIÓN EN PRODUCCIÓN
    try
    {
        using var scope = app.Services.CreateScope();
        var context = scope.ServiceProvider.GetRequiredService<Context>();

        if (await context.Database.CanConnectAsync())
        {
            Console.WriteLine("✅ Conectado a Somee correctamente");
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"❌ Error conectando a Somee: {ex.Message}");
    }
}

app.UseHttpsRedirection();
app.UseBlazorFrameworkFiles();
app.UseStaticFiles();
app.UseRouting();
app.MapRazorPages();
app.UseAuthorization();
app.MapControllers();
app.MapFallbackToFile("index.html");

Console.WriteLine("🎯 Aplicación iniciada correctamente");
app.Run();
