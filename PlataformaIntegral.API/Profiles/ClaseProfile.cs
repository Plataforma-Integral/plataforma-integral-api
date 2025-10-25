using AutoMapper;

namespace PlataformaIntegral.API.Profiles
{
    public class ClaseProfile : Profile
    {
        public ClaseProfile() 
        {
            //CREATE DTOs:
            CreateMap<DTOs.ClaseCreateDto, Models.Clase>()
                .ForMember(dest => dest.IdClase, opt => opt.Ignore())
                .ForMember(dest => dest.IdCursoSincronico, opt => opt.Ignore())
                .ForMember(dest => dest.IdProfesor, opt => opt.Ignore())
                .ForMember(dest => dest.ClasePresencial, opt => opt.Ignore())
                .ForMember(dest => dest.ClaseVirtual, opt => opt.Ignore())
                .ForMember(dest => dest.Profesor, opt => opt.Ignore());

            CreateMap<DTOs.ClasePresencialCreateDto, Models.ClasePresencial>()
                .ForMember(dest => dest.IdClase, opt => opt.Ignore())
                .ForMember(dest => dest.Clase, opt => opt.Ignore());

            CreateMap<DTOs.ClaseVirtualCreateDto, Models.ClaseVirtual>()
                .ForMember(dest => dest.IdClase, opt => opt.Ignore())
                .ForMember(dest => dest.Clase, opt => opt.Ignore());

            //READ DTOs:
            CreateMap<Models.Clase, DTOs.ClaseReadDto>()
                .ForMember(dest => dest.NombreCursoSincronico, opt => opt.MapFrom(src => src.CursoSincronico.Curso.Producto.Nombre))
                .ForMember(dest => dest.NombreProfesor, opt => opt.MapFrom(src => src.Profesor.Usuario.Nombre))
                .ForMember(dest => dest.DireccionClasePresencial, opt => opt.MapFrom(src => src.ClasePresencial != null ? src.ClasePresencial.Direccion : null))
                .ForMember(dest => dest.UrlClaseVirtual, opt => opt.MapFrom(src => src.ClaseVirtual != null ? src.ClaseVirtual.Url : null));

            CreateMap<Models.ClasePresencial, DTOs.ClasePresencialReadDto>()
                .ForMember(dest => dest.NombreClase, opt => opt.MapFrom(src => src.Clase.CursoSincronico.Curso.Producto.Nombre));

            CreateMap<Models.ClaseVirtual, DTOs.ClaseVirtualReadDto>()
                .ForMember(dest => dest.NombreClase, opt => opt.MapFrom(src => src.Clase.CursoSincronico.Curso.Producto.Nombre));

        }
    }
}
