using AutoMapper;
using PlataformaIntegral.API.DTOs;
using PlataformaIntegral.API.Models;

namespace PlataformaIntegral.API.Profiles
{
    public class RecursoProfile : Profile
    {
        public RecursoProfile() 
        {
            //CREATE DTOs:
            CreateMap<DTOs.RecursoCreateDto, Models.Recurso>()
                .ForMember(dest => dest.IdRecurso, opt => opt.Ignore())
                .ForMember(dest => dest.Video, opt => opt.Ignore())
                .ForMember(dest => dest.Certificado, opt => opt.Ignore())
                .ForMember(dest => dest.Documento, opt => opt.Ignore())
                .ForMember(dest => dest.Cuestionario, opt => opt.Ignore())
                .ForMember(dest => dest.Examen, opt => opt.Ignore())
                .ForMember(dest => dest.EstudianteProgresos, opt => opt.Ignore());

            CreateMap<DTOs.CertificadoCreateDto, Models.Certificado>()
                .ForMember(dest => dest.IdRecurso, opt => opt.Ignore())
                .ForMember(dest => dest.Curso, opt => opt.Ignore())
                .ForMember(dest => dest.Recurso, opt => opt.Ignore())
                .ForMember(dest => dest.Estudiantes, opt => opt.Ignore());

            CreateMap<DTOs.VideoCreateDto, Models.Video>()
                .ForMember(dest => dest.IdRecurso, opt => opt.Ignore())
                .ForMember(dest => dest.Recurso, opt => opt.Ignore())
                .ForMember(dest => dest.Recurso, opt => opt.Ignore())
                .ForMember(dest => dest.Documentos, opt => opt.Ignore());

            CreateMap<DTOs.DocumentoCreateDto, Models.Documento>()
                .ForMember(dest => dest.IdRecurso, opt => opt.Ignore())
                .ForMember(dest => dest.Video, opt => opt.Ignore())
                .ForMember(dest => dest.Recurso, opt => opt.Ignore());

            CreateMap<DTOs.CuestionarioCreateDto, Models.Cuestionario>()
                .ForMember(dest => dest.IdRecurso, opt => opt.Ignore())
                .ForMember(dest => dest.Recurso, opt => opt.Ignore())
                .ForMember(dest => dest.Capitulo, opt => opt.Ignore());

            CreateMap<DTOs.ExamenCreateDto, Models.Examen>()
                .ForMember(dest => dest.IdRecurso, opt => opt.Ignore())
                .ForMember(dest => dest.Recurso, opt => opt.Ignore())
                .ForMember(dest => dest.CursoPregrabado, opt => opt.Ignore());

            //READ DTOs:
            CreateMap<Models.Recurso, DTOs.RecursoReadDto>();

            CreateMap<Models.Certificado, DTOs.CertificadoReadDto>()
                .ForMember(dest => dest.NombreCurso, opt => opt.MapFrom(src => src.Curso != null ? src.Curso.Producto.Nombre : null))
                .ForMember(dest => dest.NombreRecurso, opt => opt.MapFrom(src => src.Recurso != null ? src.Recurso.Nombre : null));

            CreateMap<Models.Video, DTOs.VideoReadDto>()
                .ForMember(dest => dest.NombreRecurso, opt => opt.MapFrom(src => src.Recurso != null ? src.Recurso.Nombre : null))
                .ForMember(dest => dest.NombreCapitulo, opt => opt.MapFrom(src => src.Capitulo.Nombre));
            
            CreateMap<Models.Documento, DTOs.DocumentoReadDto>()
                .ForMember(dest => dest.NombreRecurso, opt => opt.MapFrom(src => src.Recurso != null ? src.Recurso.Nombre : null))
                .ForMember(dest => dest.NombreVideo, opt => opt.MapFrom(src => src.Video != null ? src.Video.Capitulo.Nombre : null));

            CreateMap<Models.Cuestionario, DTOs.CuestionarioReadDto>()
                .ForMember(dest => dest.NombreRecurso, opt => opt.MapFrom(src => src.Recurso != null ? src.Recurso.Nombre : null))
                .ForMember(dest => dest.NombreCapitulo, opt => opt.MapFrom(src => src.Capitulo != null ? src.Capitulo.Nombre : null));

            CreateMap<Models.Examen, DTOs.ExamenReadDto>()
                .ForMember(dest => dest.NombreRecurso, opt => opt.MapFrom(src => src.Recurso != null ? src.Recurso.Nombre : null))
                .ForMember(dest => dest.NombreCursoPregrabado, opt => opt.MapFrom(src => src.CursoPregrabado != null ? src.CursoPregrabado.Curso.Producto.Nombre : null));

            CreateMap<Cuestionario, RecursoCuestionarioDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.IdRecurso))
                .ForMember(dest => dest.Titulo, opt => opt.MapFrom(src => src.Recurso.Nombre))
                .ForMember(dest => dest.NumeroOrden, opt => opt.MapFrom(src => src.NumeroOrden))
                .ForMember(dest => dest.Resuelto, opt => opt.Ignore()); // Se asignará en el servicio según el estudiante autenticado.

            CreateMap<Video, RecursoVideoDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.IdRecurso))
                .ForMember(dest => dest.Titulo, opt => opt.MapFrom(src => src.Recurso.Nombre))
                .ForMember(dest => dest.Duracion, opt => opt.Ignore())
                .ForMember(dest => dest.NumeroOrden, opt => opt.MapFrom(src => src.NumeroOrden))
                .ForMember(dest => dest.Visto, opt => opt.Ignore()) // Se asignará en el servicio según el estudiante autenticado.
                .ForMember(dest => dest.MiniaturaUrl, opt => opt.Ignore()); // Se asignará en el servicio si es necesario.


        }
    }
}
