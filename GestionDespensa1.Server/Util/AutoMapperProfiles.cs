using AutoMapper;
using GestionDespensa1.BD.Data.Entity;
using GestionDespensa1.Shared.DTO;

namespace GestionDespensa1.Server.Util
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            // Mapeos para Cliente - CORREGIDOS
            CreateMap<CrearClienteDTO, Cliente>().ReverseMap();
            CreateMap<ClienteDTO, Cliente>().ReverseMap();

            // Tus otros mapeos...
            CreateMap<CrearCajaDTO, Caja>().ReverseMap();
            CreateMap<CajaDTO, Caja>().ReverseMap();
            CreateMap<CrearCategoriaDTO, Categoria>().ReverseMap();
            CreateMap<CategoriaDTO, Categoria>().ReverseMap();
            //CreateMap<CrearCompraProveedorDTO, CompraProveedor>().ReverseMap();
            //CreateMap<CompraProveedorDTO, CompraProveedor>().ReverseMap();
            CreateMap<CrearDetalleCajaDTO, DetalleCaja>().ReverseMap();
            CreateMap<DetalleCajaDTO, DetalleCaja>().ReverseMap();
            //CreateMap<CrearDetalleCompraProveedorDTO, DetalleCompraProveedor>().ReverseMap();
            //CreateMap<DetalleCompraProveedorDTO, DetalleCompraProveedor>().ReverseMap();
            CreateMap<CrearDetalleVentaDTO, DetalleVenta>().ReverseMap();
            CreateMap<DetalleVentaDTO, DetalleVenta>().ReverseMap();
            CreateMap<CrearProductoDTO, Producto>().ReverseMap();
            CreateMap<ProductoDTO, Producto>().ReverseMap();
           // CreateMap<CrearProveedorDTO, Proveedor>().ReverseMap();
            //CreateMap<ProveedorDTO, Proveedor>().ReverseMap();//
            CreateMap<CrearVentaDTO, Venta>().ReverseMap();
            CreateMap<VentaDTO, Venta>().ReverseMap();
            CreateMap<Proveedor, ProveedorDTO>()
                .ForMember(dest => dest.Productos, opt => opt.MapFrom(src => src.Notas)) // Los productos están en Notas
                .ReverseMap()
                .ForMember(dest => dest.Notas, opt => opt.MapFrom(src => src.Notas)); // Mantener notas originales

            CreateMap<CrearProveedorDTO, Proveedor>();
            CreateMap<CompraProveedor, CompraProveedorDTO>().ReverseMap();
            CreateMap<DetalleCompraProveedor, DetalleCompraProveedorDTO>().ReverseMap();
        }
    }
}