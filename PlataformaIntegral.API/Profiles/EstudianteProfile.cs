using AutoMapper;
using PlataformaIntegral.API.DTOs;
using PlataformaIntegral.API.Models;

namespace PlataformaIntegral.API.Profiles
{
    public class EstudianteProfile : Profile
    {
        public EstudianteProfile() 
        {
            //CREATE DTOs:
            CreateMap<EstudianteCreateDto, Estudiante>()
                .ForMember(dest => dest.ReseñaCursos, opt => opt.Ignore())
                .ForMember(dest => dest.ReseñaProfesores, opt => opt.Ignore())
                .ForMember(dest => dest.EstudianteMedallas, opt => opt.Ignore())
                .ForMember(dest => dest.EstudianteProgresos, opt => opt.Ignore())
                .ForMember(dest => dest.Categorias, opt => opt.Ignore())
                .ForMember(dest => dest.Certificados, opt => opt.Ignore())
                .ForMember(dest => dest.Torneos, opt => opt.Ignore())
                .ForMember(dest => dest.Usuario, opt => opt.Ignore())
                .ForMember(dest => dest.UltimoLogin, opt => opt.Ignore())
                .ForMember(dest => dest.IdUsuario, opt => opt.Ignore());

            CreateMap<EstudianteProgresoCreateDto, EstudianteProgreso>()
                .ForMember(dest => dest.Estudiante, opt => opt.Ignore())
                .ForMember(dest => dest.Recurso, opt => opt.Ignore());

            //READ DTOs:
            CreateMap<Estudiante, EstudianteReadDto>()
                .ForMember(dest => dest.NombreUsuario, opt => opt.MapFrom(src => src.Usuario.Nombre));

            CreateMap<EstudianteProgreso, EstudianteProgresoReadDto>()
                .ForMember(dest => dest.NombreEstudiante, opt => opt.MapFrom(src => src.Estudiante.Usuario.Nombre))
                .ForMember(dest => dest.NombreRecurso, opt => opt.MapFrom(src => src.Recurso.Nombre));
        }
    }
}
