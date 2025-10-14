using GestionDespensa1.BD.Data;
using GestionDespensa1.Server.Repositorio;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;

//------------------------------------------------------------------
//configuracion de los servicios en el constructor de la aplicaci�n
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers().AddJsonOptions(
    x => x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//builder.Services.AddDbContext<Context>(op => op.UseSqlServer("name=conn"));//
builder.Services.AddDbContext<Context>(op => op.UseSqlServer("name=conn1"));

builder.Services.AddAutoMapper(typeof(Program));

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
//--------------------------------------------------------------------
//construcc�n de la aplicaci�n
var app = builder.Build();

// Configure the HTTP request pipeline.
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