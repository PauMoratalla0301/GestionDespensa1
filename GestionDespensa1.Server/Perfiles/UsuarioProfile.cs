using AutoMapper;
using GestionDespensa1.BD.Data.Entity;
using GestionDespensa1.Shared.DTO;

namespace GestionDespensa1.Server.Perfiles
{
    public class UsuarioProfile : Profile
    {
        public UsuarioProfile()
        {
            CreateMap<Usuario, UsuarioDTO>();
            CreateMap<CrearUsuarioDTO, Usuario>();
        }
    }
}