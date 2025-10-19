using AutoMapper;
using PlataformaIntegral.API.DTOs;
using PlataformaIntegral.API.Models;

namespace PlataformaIntegral.API.Profiles
{
    public class UsuarioProfile : Profile
    {
        public UsuarioProfile()
        {
            // Mapeo automático
            CreateMap<UsuarioCreateDto, Usuario>();
            CreateMap<CredencialCreateDto, Credencial>();
            CreateMap<ConfigPrivacidadCreateDto, ConfiguracionPrivacidad>();

            // Mapeo manual para ReadDto (planos)
            CreateMap<Usuario, UsuarioReadDto>()
                .ForMember(dest => dest.Pais, opt => opt.MapFrom(src => src.Pais != null ? src.Pais.Nombre : null))
                .ForMember(dest => dest.TipoUsuario, opt => opt.MapFrom(src => src.TipoUsuario != null ? src.TipoUsuario.Nombre : null))
                .ForMember(dest => dest.ConfiguracionPrivacidad, opt => opt.MapFrom(src => src.ConfiguracionPrivacidad));
            CreateMap<ConfiguracionPrivacidad, ConfigPrivacidadReadDto>();

        }
    }
}
