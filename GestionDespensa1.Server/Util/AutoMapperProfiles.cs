using AutoMapper;
using GestionDespensa1.BD.Data.Entity;
using GestionDespensa1.Shared.DTO;
using Microsoft.Extensions.Configuration;

namespace GestionDespensa1.Server.Util
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            // ============ CLIENTES ============
            CreateMap<CrearClienteDTO, Cliente>().ReverseMap();
            CreateMap<ClienteDTO, Cliente>().ReverseMap();

            // ============ USUARIOS ============
            CreateMap<Usuario, UsuarioDTO>();
            CreateMap<CrearUsuarioDTO, Usuario>();

            // ============ CAJA ============
            CreateMap<Caja, CajaDTO>()
                .ForMember(dest => dest.NombreUsuario,
                          opt => opt.MapFrom(src => src.Usuario != null ? src.Usuario.Nombre : ""));
            CreateMap<CrearCajaDTO, Caja>();
            CreateMap<CajaDTO, Caja>();

            // ============ CATEGORIAS ============
            CreateMap<CrearCategoriaDTO, Categoria>().ReverseMap();
            CreateMap<CategoriaDTO, Categoria>().ReverseMap();

            // ============ DETALLE CAJA ============
            CreateMap<CrearDetalleCajaDTO, DetalleCaja>().ReverseMap();
            CreateMap<DetalleCajaDTO, DetalleCaja>().ReverseMap();

            // ============ DETALLE VENTA ============
            CreateMap<DetalleVenta, DetalleVentaDTO>()
                .ForMember(dest => dest.DescripcionProducto,
                          opt => opt.MapFrom(src => src.Producto != null ? src.Producto.Descripcion : ""));
            CreateMap<CrearDetalleVentaDTO, DetalleVenta>();

            // ============ PRODUCTOS ============
            CreateMap<CrearProductoDTO, Producto>().ReverseMap();
            CreateMap<ProductoDTO, Producto>().ReverseMap();

            // ============ PROVEEDORES ============
            CreateMap<Proveedor, ProveedorDTO>()
                .ForMember(dest => dest.Productos, opt => opt.MapFrom(src => src.Notas))
                .ReverseMap()
                .ForMember(dest => dest.Notas, opt => opt.MapFrom(src => src.Notas));
            CreateMap<CrearProveedorDTO, Proveedor>();

            // ============ COMPRAS ============
            CreateMap<CompraProveedor, CompraProveedorDTO>().ReverseMap();
            CreateMap<DetalleCompraProveedor, DetalleCompraProveedorDTO>().ReverseMap();

            // ============ VENTAS ============ ✅ CORREGIDO
            CreateMap<Venta, VentaDTO>()
                .ForMember(dest => dest.NombreCliente,
                          opt => opt.MapFrom(src => src.Cliente != null ? src.Cliente.Nombre + " " + src.Cliente.Apellido : ""))
                .ForMember(dest => dest.NombreUsuario,
                          opt => opt.MapFrom(src => src.Usuario != null ? src.Usuario.Nombre : ""));

            CreateMap<VentaDTO, Venta>()
                .ForMember(dest => dest.Cliente, opt => opt.Ignore())
                .ForMember(dest => dest.Usuario, opt => opt.Ignore())
                .ForMember(dest => dest.DetallesVenta, opt => opt.Ignore());

            CreateMap<CrearVentaDTO, Venta>()
                .ForMember(dest => dest.DetallesVenta, opt => opt.Ignore()); // 👈 ESTO ES CLAVE
        }
    }
}