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
            CreateMap<DTOs.CursoSincronicoCreateDto, Models.CursoSincronico>()
                .ForMember(dest => dest.IdCurso, opt => opt.Ignore())
                .ForMember(dest => dest.Curso, opt => opt.Ignore())
                .ForMember(dest => dest.Clases, opt => opt.Ignore())
                .ForMember(dest => dest.Modalidad, opt => opt.Ignore());

            CreateMap<DTOs.ModalidadSincronicoCreateDto, Models.ModalidadSincronico>()
                .ForMember(dest => dest.IdModalidad, opt => opt.Ignore())
                .ForMember(dest => dest.CursoSincronicos, opt => opt.Ignore());

            //READ DTOs:
            CreateMap<Models.CursoSincronico, DTOs.CursoSincronicoReadDto>()
                .ForMember(dest => dest.NombreCurso, opt => opt.MapFrom(src => src.Curso.Producto.Nombre))
                .ForMember(dest => dest.NombreModalidad, opt => opt.MapFrom(src => src.Modalidad != null ? src.Modalidad.Nombre : null));

            CreateMap<Models.ModalidadSincronico, DTOs.ModalidadSincronicoReadDto>();
        }
    }
}
