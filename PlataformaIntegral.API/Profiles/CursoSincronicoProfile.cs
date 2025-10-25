using AutoMapper;
using PlataformaIntegral.API.DTOs;
using PlataformaIntegral.API.Models;

namespace PlataformaIntegral.API.Profiles
{
    public class CursoSincronicoProfile : Profile
    {
        public CursoSincronicoProfile()
        {
            //CREATE DTOs:
            CreateMap<CursoSincronicoCreateDto, CursoSincronico>()
                .ForMember(dest => dest.IdCurso, opt => opt.Ignore())
                .ForMember(dest => dest.Curso, opt => opt.Ignore())
                .ForMember(dest => dest.Clases, opt => opt.Ignore())
                .ForMember(dest => dest.Modalidad, opt => opt.Ignore());
            //READ DTOs:
            CreateMap<CursoSincronico, CursoSincronicoReadDto>()
                .ForMember(dest => dest.NombreCurso, opt => opt.MapFrom(src => src.Curso.Producto.Nombre))
                .ForMember(dest => dest.NombreModalidad, opt => opt.MapFrom(src => src.Modalidad != null ? src.Modalidad.Nombre : null));
        }
    }
}
