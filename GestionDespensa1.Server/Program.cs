using GestionDespensa1.BD.Data;
using GestionDespensa1.Server.Repositorio;
using GestionDespensa1.Server.Servicios;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;                       
using System.Text;
using System.Text.Json.Serialization;

var hasher = new PasswordHasher<object>();
var hash = hasher.HashPassword(null, "123456");
Console.WriteLine(hash);

var builder = WebApplication.CreateBuilder(args);

// ============ 1. CONFIGURACIÓN JWT (AGREGAR ANTES DE LOS SERVICIOS) ============
var jwtKey = builder.Configuration["Jwt:Key"] ?? "MiClaveSuperSecretaParaJWT2026Con32Caracteres!";
var jwtIssuer = builder.Configuration["Jwt:Issuer"] ?? "GestionDespensa";
var jwtAudience = builder.Configuration["Jwt:Audience"] ?? "GestionDespensaClient";

Console.WriteLine($"🔐 JWT Configurado - Issuer: {jwtIssuer}");

// ============ 2. AGREGAR AUTENTICACIÓN JWT ============
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtIssuer,
            ValidAudience = jwtAudience,
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(jwtKey))
        };
    });

builder.Services.AddAuthorization(); // 👈 AGREGAR AUTORIZACIÓN

// ============ 3. TUS SERVICIOS EXISTENTES (SIN CAMBIOS) ============
builder.Services.AddControllers().AddJsonOptions(
    x => x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddScoped<DashboardService>();
builder.Services.AddScoped<ReportesService>();
builder.Services.AddScoped<PdfService>();
builder.Services.AddScoped<NotificacionesService>();
builder.Services.AddScoped<ExcelService>();
builder.Services.AddScoped<PdfVentaService>();
builder.Services.AddScoped<BackupService>();
builder.Services.AddHostedService<BackupSchedulerService>();
builder.Services.AddScoped<FidelizacionService>();

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

// ============ 4. REGISTRAR REPOSITORIOS (AGREGAR EL NUEVO) ============
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
builder.Services.AddScoped<IUsuarioRepositorio, UsuarioRepositorio>();
builder.Services.AddScoped<IMovimientoStockRepositorio, MovimientoStockRepositorio>();
builder.Services.AddScoped<IPagoVentaRepositorio, PagoVentaRepositorio>();
builder.Services.AddScoped<IPagoProveedorRepositorio, PagoProveedorRepositorio>();

var app = builder.Build();

// ============ 5. VERIFICACIÓN DE BD (TU CÓDIGO EXISTENTE) ============
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

// ============ 6. CONFIGURACIÓN DE PIPELINE (AGREGAR UseAuthentication) ============
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

app.UseAuthentication();  // 👈 NUEVO (DEBE IR ANTES DE UseAuthorization)
app.UseAuthorization();   // 👈 YA EXISTE

app.MapRazorPages();
app.MapControllers();
app.MapFallbackToFile("index.html");

Console.WriteLine("🎯 Aplicación iniciada correctamente");
app.Run();