using AutoMapper;
using PlataformaIntegral.API.DTOs;
using PlataformaIntegral.API.Models;

namespace PlataformaIntegral.API.Profiles
{
    public class CursoPregrabadoProfile : Profile
    {
        public CursoPregrabadoProfile()
        {
            //CREATE DTOs:
            CreateMap<CursoPregrabadoCreateDto, CursoPregrabado>()
                .ForMember(dest => dest.IdCurso, opt => opt.Ignore())
                .ForMember(dest => dest.Curso, opt => opt.Ignore())
                .ForMember(dest => dest.Capitulos, opt => opt.Ignore())
                .ForMember(dest => dest.Examen, opt => opt.Ignore());

            CreateMap<CapituloCreateDto, Capitulo>()
                .ForMember(dest => dest.IdCapitulo, opt => opt.Ignore())
                .ForMember(dest => dest.IdCursoPregrabado, opt => opt.Ignore())
                .ForMember(dest => dest.CursoPregrabado, opt => opt.Ignore())
                .ForMember(dest => dest.Cuestionarios, opt => opt.Ignore())
                .ForMember(dest => dest.Videos, opt => opt.Ignore());

            //READ DTOs:
            CreateMap<CursoPregrabado, CursoPregrabadoReadDto>()
                .ForMember(dest => dest.NombreCurso, opt => opt.MapFrom(src => src.Curso.Producto.Nombre));

            CreateMap<Capitulo, CapituloReadDto>()
                .ForMember(dest => dest.NombreCursoPregrabado, opt => opt.MapFrom(src => src.CursoPregrabado.Curso.Producto.Nombre));
        }
    }
}
