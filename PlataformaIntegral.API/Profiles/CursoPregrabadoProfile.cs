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

            CreateMap<CursoPregrabado, CursoPaginaDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.IdCurso))
                .ForMember(dest => dest.Titulo, opt => opt.MapFrom(src => src.Curso.Producto.Nombre))
                .ForMember(dest => dest.PortadaUrl, opt => opt.MapFrom(src => src.Curso.CursoPregrabado.UrlPortada))
                .ForMember(dest => dest.Precio, opt => opt.MapFrom(src => src.Curso.Producto.Precio))
                .ForMember(dest => dest.Descripcion, opt => opt.MapFrom(src => src.Curso.Producto.Descripcion))
                .ForMember(dest => dest.Categorias, opt => opt.MapFrom(src => src.Curso.Categorias.Select(cp => cp.Nombre).ToList()))
                .ForMember(dest => dest.Capitulos, opt => opt.MapFrom(src => src.Capitulos))
                .ForMember(dest => dest.FechaCreacion, opt => opt.MapFrom(src => src.Curso.Producto.FechaCreacion))
                .ForMember(dest => dest.Calificacion, opt => opt.MapFrom(src =>
                    src.Curso.ReseñaCursos.Any()
                        ? (double?)src.Curso.ReseñaCursos.Count(r => r.Opinion == true)
                            / src.Curso.ReseñaCursos.Count() * 100.0
                        : null
                ))
                .ForMember(dest => dest.Reseña, opt => opt.Ignore()) // Se asignará en el servicio según el estudiante autenticado.
                .ForMember(dest => dest.Profesores, opt => opt.MapFrom(src => src.Curso.ProfesorCursos.Select(pc => pc.Profesor).ToList()));

            CreateMap<Capitulo, CapituloDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.IdCapitulo))
                .ForMember(dest => dest.Titulo, opt => opt.MapFrom(src => src.Nombre))
                .ForMember(dest => dest.NumeroOrden, opt => opt.MapFrom(src => src.NumeroOrden));
        }
    }
}
