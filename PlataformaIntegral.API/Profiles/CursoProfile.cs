using AutoMapper;
using PlataformaIntegral.API.DTOs;
using PlataformaIntegral.API.Models;

namespace PlataformaIntegral.API.Profiles
{
    public class CursoProfile : Profile
    {
        public CursoProfile() 
        {
            //CREATE DTOs:
            CreateMap<CursoCreateDto, Curso>()
                .ForMember(dest => dest.IdProducto, opt => opt.Ignore())
                .ForMember(dest => dest.Certificados, opt => opt.Ignore())
                .ForMember(dest => dest.CursoPregrabado, opt => opt.Ignore())
                .ForMember(dest => dest.CursoSincronico, opt => opt.Ignore())
                .ForMember(dest => dest.Producto, opt => opt.Ignore())
                .ForMember(dest => dest.ProfesorCursos, opt => opt.Ignore())
                .ForMember(dest => dest.ReseñaCursos, opt => opt.Ignore())
                .ForMember(dest => dest.Categorias, opt => opt.Ignore())
                .ForMember(dest => dest.SuscripcionesTipo, opt => opt.Ignore());

            CreateMap<ResenaCursoCreateDto, ResenaCurso>()
                .ForMember(dest => dest.Curso, opt => opt.Ignore())
                .ForMember(dest => dest.Estudiante, opt => opt.Ignore());

            //READ DTOs:
            CreateMap<Curso, CursoReadDto>()
                .ForMember(dest => dest.NombreProducto, opt => opt.MapFrom(src => src.Producto.Nombre));

            CreateMap<ResenaCurso, ResenaCursoReadDto>()
                .ForMember(dest => dest.NombreCurso, opt => opt.MapFrom(src => src.Curso.Producto.Nombre))
                .ForMember(dest => dest.NombreEstudiante, opt => opt.MapFrom(src => src.Estudiante.Usuario.Nombre));

            CreateMap<Curso, CursoCardDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.IdProducto))
                .ForMember(dest => dest.Titulo, opt => opt.MapFrom(src => src.Producto.Nombre))
                .ForMember(dest => dest.Precio, opt => opt.MapFrom(src => src.Producto.Precio))
                .ForMember(dest => dest.PortadaUrl, opt => opt.MapFrom(src => src.CursoPregrabado != null ? src.CursoPregrabado.UrlPortada : "curso Sincronico")) //Añadir portada para el curso sincrónico.
                .ForMember(dest => dest.Categorias, opt => opt.MapFrom(src => src.Categorias.Select(c => c.Nombre).ToList()))
                .ForMember(dest => dest.Profesores, opt => opt.MapFrom(src => src.ProfesorCursos.Select(pc => pc.Profesor).ToList()))
                .ForMember(dest => dest.Calificacion, opt => opt.MapFrom(src =>
                    src.ReseñaCursos.Any()
                        ? (double?)src.ReseñaCursos.Count(r => r.Opinion == true)
                            / src.ReseñaCursos.Count() * 100.0
                        : null
                ))
                .ForMember(dest => dest.CantidadEstudiantes, opt => opt.MapFrom(src =>
                    src.Producto.Pagos != null
                        ? src.Producto.Pagos
                            .Where(p => p.EstadoPago.Nombre == "Aprobado")
                            .Select(p => p.Recibo.IdUsuario)
                            .Distinct()
                            .Count()
                        : 0
                ))
                .ForMember(dest => dest.Modalidad, opt => opt.MapFrom(src => src.CursoPregrabado != null ? "Pregrabado" : (src.CursoSincronico != null ? "Sincrónico" : null)))
                .ForMember(dest => dest.FechaPublicacion, opt => opt.MapFrom(src => src.Producto.FechaCreacion));
        }
    }
}
