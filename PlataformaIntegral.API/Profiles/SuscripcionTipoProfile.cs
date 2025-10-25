using AutoMapper;

namespace PlataformaIntegral.API.Profiles
{
    public class SuscripcionTipoProfile : Profile
    {
        public SuscripcionTipoProfile()
        {
            //CREATE DTOs:
            CreateMap<DTOs.SuscripcionTipoCreateDto, Models.SuscripcionTipo>()
                .ForMember(dest => dest.IdProducto, opt => opt.Ignore())
                .ForMember(dest => dest.Producto, opt => opt.Ignore())
                .ForMember(dest => dest.Suscripciones, opt => opt.Ignore())
                .ForMember(dest => dest.Cursos, opt => opt.Ignore());

            CreateMap<DTOs.SuscripcionCreateDto, Models.Suscripcion>()
                .ForMember(dest => dest.IdSuscripcion, opt => opt.Ignore())
                .ForMember(dest => dest.FechaInicio, opt => opt.Ignore())
                .ForMember(dest => dest.SuscripcionTipo, opt => opt.Ignore())
                .ForMember(dest => dest.Usuario, opt => opt.Ignore())
                .ForMember(dest => dest.EstadoSuscripcion, opt => opt.Ignore());

            CreateMap<DTOs.EstadoSuscripcionCreateDto, Models.EstadoSuscripcion>()
                .ForMember(dest => dest.IdEstadoSuscripcion, opt => opt.Ignore())
                .ForMember(dest => dest.Suscripciones, opt => opt.Ignore());

            //READ DTOs:
            CreateMap<Models.SuscripcionTipo, DTOs.SuscripcionTipoReadDto>()
                .ForMember(dest => dest.NombreProducto, opt => opt.MapFrom(src => src.Producto.Nombre));

            CreateMap<Models.Suscripcion, DTOs.SuscripcionReadDto>()
                .ForMember(dest => dest.NombreEstadoSuscripcion, opt => opt.MapFrom(src => src.EstadoSuscripcion != null ? src.EstadoSuscripcion.Nombre : null))
                .ForMember(dest => dest.NombreSuscripcionTipo, opt => opt.MapFrom(src => src.SuscripcionTipo != null ? src.SuscripcionTipo.Producto.Nombre : null))
                .ForMember(dest => dest.NombreUsuario, opt => opt.MapFrom(src => src.Usuario != null ? src.Usuario.Nombre : null));

            CreateMap<Models.EstadoSuscripcion, DTOs.EstadoSuscripcionReadDto>();
        }
    }
}
