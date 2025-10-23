using AutoMapper;
using PlataformaIntegral.API.DTOs;
using PlataformaIntegral.API.Models;

namespace PlataformaIntegral.API.Profiles
{
    public class UsuarioProfile : Profile
    {
        public UsuarioProfile()
        {
            //CREATE DTOs:
            CreateMap<UsuarioCreateDto, Usuario>();
            //Herencia:
            CreateMap<AdministradorCreateDto, Administrador>();
            //Relacionados con Usuario:
            CreateMap<CredencialCreateDto, Credencial>();
            CreateMap<ConfigPrivacidadCreateDto, ConfiguracionPrivacidad>();
            CreateMap<TipoUsuarioCreateDto, TipoUsuario>();
            CreateMap<PaisCreateDto, Pais>();

            //READ DTOs:
            // Mapeo manual para ReadDto (planos)
            CreateMap<Usuario, UsuarioReadDto>()
                .ForMember(dest => dest.Pais, opt => opt.MapFrom(src => src.Pais != null ? src.Pais.Nombre : null))
                .ForMember(dest => dest.TipoUsuario, opt => opt.MapFrom(src => src.TipoUsuario != null ? src.TipoUsuario.Nombre : null))
                .ForMember(dest => dest.ConfiguracionPrivacidad, opt => opt.MapFrom(src => src.ConfiguracionPrivacidad));
            //Herencia:
            CreateMap<Administrador, AdministradorReadDto>()
                .ForMember(dest => dest.NombreUsuario, opt => opt.MapFrom(src => src.Usuario.Nombre));
            //Relacionados con Usuario:
            CreateMap<Credencial, CredencialReadDto>()
                .ForMember(dest => dest.NombreUsuario, opt => opt.MapFrom(src => src.Usuario.Nombre));
            CreateMap<ConfiguracionPrivacidad, ConfigPrivacidadReadDto>();
            CreateMap<TipoUsuario, TipoUsuarioReadDto>();
            CreateMap<Pais, PaisReadDto>();
        }
    }
}
