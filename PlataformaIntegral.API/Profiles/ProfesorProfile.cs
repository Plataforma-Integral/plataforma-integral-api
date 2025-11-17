using AutoMapper;
using PlataformaIntegral.API.DTOs;
using PlataformaIntegral.API.Models;

namespace PlataformaIntegral.API.Profiles
{
    public class ProfesorProfile : Profile
    {
        public ProfesorProfile() 
        {
            //CREATE DTOs:
            CreateMap<ProfesorCreateDto, Profesor>()
                .ForMember(dest => dest.IdUsuario, opt => opt.Ignore())
                .ForMember(dest => dest.Clases, opt => opt.Ignore())
                .ForMember(dest => dest.Usuario, opt => opt.Ignore())
                .ForMember(dest => dest.Categorias, opt => opt.Ignore())
                .ForMember(dest => dest.ProfesorCursos, opt => opt.Ignore())
                .ForMember(dest => dest.ReseñasProfesor, opt => opt.Ignore());

            CreateMap<ResenaProfesorCreateDto, ResenaProfesor>()
                .ForMember(dest => dest.Profesor, opt => opt.Ignore())
                .ForMember(dest => dest.Estudiante, opt => opt.Ignore());

            CreateMap<ProfesorCursoCreateDto, ProfesorCurso>()
                .ForMember(dest => dest.Profesor, opt => opt.Ignore())
                .ForMember(dest => dest.Curso, opt => opt.Ignore())
                .ForMember(dest => dest.TipoRol, opt => opt.Ignore());

            //READ DTOs:
            CreateMap<Profesor, ProfesorReadDto>()
                .ForMember(dest => dest.NombreUsuario, opt => opt.MapFrom(src => src.Usuario.Nombre));

            CreateMap<ResenaProfesor, ResenaProfesorReadDto>()
                .ForMember(dest => dest.NombreEstudiante, opt => opt.MapFrom(src => src.Estudiante.Usuario.Nombre))
                .ForMember(dest => dest.NombreProfesor, opt => opt.MapFrom(src => src.Profesor.Usuario.Nombre));

            CreateMap<ProfesorCurso, ProfesorCursoReadDto>()
                .ForMember(dest => dest.NombreProfesor, opt => opt.MapFrom(src => src.Profesor.Usuario.Nombre))
                .ForMember(dest => dest.NombreCurso, opt => opt.MapFrom(src => src.Curso.Producto.Nombre))
                .ForMember(dest => dest.NombreTipoRol, opt => opt.MapFrom(src => src.TipoRol != null ? src.TipoRol.Nombre : null));

            CreateMap<Profesor, ProfesorSimpleDto>()
                .ForMember(dest => dest.NombreCompleto, opt => opt.MapFrom(src => src.Usuario.Nombre))
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.IdUsuario));
        }
    }
}
