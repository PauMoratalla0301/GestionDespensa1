using GestionDespensa1.BD.Data;
using GestionDespensa1.Server.Repositorio;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers().AddJsonOptions(
    x => x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

Console.WriteLine($"🎯 Entorno: {builder.Environment.EnvironmentName}");
Console.WriteLine($"🔗 Cadena conexión: {connectionString}");

builder.Services.AddDbContext<Context>(options =>
{
    options.UseSqlServer(connectionString, sqlOptions =>
    {
        sqlOptions.EnableRetryOnFailure(
            maxRetryCount: 3,
            maxRetryDelay: TimeSpan.FromSeconds(5),
            errorNumbersToAdd: null);
    });
});

builder.Services.AddAutoMapper(typeof(Program).Assembly);


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

try
{
    using var scope = app.Services.CreateScope();
    var context = scope.ServiceProvider.GetRequiredService<Context>();

    Console.WriteLine("🔄 Verificando conexión a la base de datos...");

    if (await context.Database.CanConnectAsync())
    {
        Console.WriteLine("✅ Conexión exitosa con la base de datos");

        if (app.Environment.IsDevelopment())
        {
            Console.WriteLine("🔄 Verificando migraciones...");
            var pendingMigrations = await context.Database.GetPendingMigrationsAsync();
            if (pendingMigrations.Any())
            {
                Console.WriteLine($"📦 Aplicando {pendingMigrations.Count()} migraciones pendientes...");
                await context.Database.MigrateAsync();
                Console.WriteLine("✅ Migraciones aplicadas correctamente");
            }
            else
            {
                Console.WriteLine("✅ No hay migraciones pendientes");
            }
        }
    }
    else
    {
        Console.WriteLine("❌ No se pudo conectar a la base de datos");
    }
}
catch (Exception ex)
{
    Console.WriteLine($"💥 ERROR durante la verificación de conexión: {ex.Message}");
    if (ex.InnerException != null)
    {
        Console.WriteLine($"🔍 Inner Exception: {ex.InnerException.Message}");
    }
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseDeveloperExceptionPage();
    Console.WriteLine("🚀 MODO DESARROLLO");
}
else
{
    Console.WriteLine("🚀 MODO PRODUCCIÓN");
    app.UseHsts(); 
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