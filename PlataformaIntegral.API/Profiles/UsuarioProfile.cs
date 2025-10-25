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
            CreateMap<UsuarioCreateDto, Usuario>()
                .ForMember(dest => dest.IdUsuario, opt => opt.Ignore())
                .ForMember(dest => dest.Administrador, opt => opt.Ignore())
                .ForMember(dest => dest.ConfiguracionPrivacidad, opt => opt.Ignore())
                .ForMember(dest => dest.Credenciales, opt => opt.Ignore())
                .ForMember(dest => dest.Pais, opt => opt.Ignore())
                .ForMember(dest => dest.TipoUsuario, opt => opt.Ignore())
                .ForMember(dest => dest.Pagos, opt => opt.Ignore())
                .ForMember(dest => dest.Profesor, opt => opt.Ignore())
                .ForMember(dest => dest.Suscripciones, opt => opt.Ignore());
                //Relacionados con Usuario:
            CreateMap<CredencialCreateDto, Credencial>()
                .ForMember(dest => dest.IdCredencial, opt => opt.Ignore())
                .ForMember(dest => dest.IdUsuario, opt => opt.Ignore())
                .ForMember(dest => dest.FechaCreacion, opt => opt.Ignore())
                .ForMember(dest => dest.Usuario, opt => opt.Ignore());
            CreateMap<ConfigPrivacidadCreateDto, ConfiguracionPrivacidad>()
                .ForMember(dest => dest.IdUsuario, opt => opt.Ignore())
                .ForMember(dest => dest.Usuario, opt => opt.Ignore());
            CreateMap<TipoUsuarioCreateDto, TipoUsuario>()
                .ForMember(dest => dest.IdTipoUsuario, opt => opt.Ignore())
                .ForMember(dest => dest.Usuarios, opt => opt.Ignore());
            CreateMap<PaisCreateDto, Pais>()
                .ForMember(dest => dest.IdPais, opt => opt.Ignore())
                .ForMember(dest => dest.Usuarios, opt => opt.Ignore());

            //READ DTOs:
            CreateMap<Usuario, UsuarioReadDto>()
                .ForMember(dest => dest.Pais, opt => opt.MapFrom(src => src.Pais != null ? src.Pais.Nombre : null))
                .ForMember(dest => dest.TipoUsuario, opt => opt.MapFrom(src => src.TipoUsuario != null ? src.TipoUsuario.Nombre : null))
                .ForMember(dest => dest.ConfiguracionPrivacidad, opt => opt.MapFrom(src => src.ConfiguracionPrivacidad));
                //Relacionados con Usuario:
            CreateMap<Credencial, CredencialReadDto>()
                .ForMember(dest => dest.NombreUsuario, opt => opt.MapFrom(src => src.Usuario.Nombre));
            CreateMap<ConfiguracionPrivacidad, ConfigPrivacidadReadDto>();
            CreateMap<TipoUsuario, TipoUsuarioReadDto>();
            CreateMap<Pais, PaisReadDto>();
        }
    }
}
