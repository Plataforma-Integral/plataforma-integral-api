using AutoMapper;

namespace PlataformaIntegral.API.Profiles
{
    public class TorneoProfile : Profile
    {
        public TorneoProfile() 
        {
            // CREATE DTOs:
            CreateMap<DTOs.TorneoCreateDto, Models.Torneo>()
                .ForMember(dest => dest.IdTorneo, opt => opt.Ignore())
                .ForMember(dest => dest.AdministradorTorneos, opt => opt.Ignore())
                .ForMember(dest => dest.Medallas, opt => opt.Ignore())
                .ForMember(dest => dest.Categorias, opt => opt.Ignore())
                .ForMember(dest => dest.Estudiantes, opt => opt.Ignore());

            CreateMap<DTOs.MedallaCreateDto, Models.Medalla>()
                .ForMember(dest => dest.IdMedalla, opt => opt.Ignore())
                .ForMember(dest => dest.EstudianteMedallas, opt => opt.Ignore())
                .ForMember(dest => dest.NivelMedalla, opt => opt.Ignore())
                .ForMember(dest => dest.Torneo, opt => opt.Ignore());

            CreateMap<DTOs.NivelMedallaCreateDto, Models.NivelMedalla>()
                .ForMember(dest => dest.IdNivelMedalla, opt => opt.Ignore())
                .ForMember(dest => dest.Medallas, opt => opt.Ignore());

            CreateMap<DTOs.AdministradorTorneoCreateDto, Models.AdministradorTorneo>()
                .ForMember(dest => dest.Torneo, opt => opt.Ignore())
                .ForMember(dest => dest.Administrador, opt => opt.Ignore())
                .ForMember(dest => dest.TipoRol, opt => opt.Ignore());

            CreateMap<DTOs.EstudianteMedallaCreateDto, Models.EstudianteMedalla>()
                .ForMember(dest => dest.FechaOtorgada, opt => opt.Ignore())
                .ForMember(dest => dest.Medalla, opt => opt.Ignore())
                .ForMember(dest => dest.Estudiante, opt => opt.Ignore());

            // READ DTOs:
            CreateMap<Models.Torneo, DTOs.TorneoReadDto>();

            CreateMap<Models.Medalla, DTOs.MedallaReadDto>()
                .ForMember(dest => dest.NombreNivelMedalla, opt => opt.MapFrom(src => src.NivelMedalla != null ? src.NivelMedalla.Nombre : null))
                .ForMember(dest => dest.NombreTorneo, opt => opt.MapFrom(src => src.Torneo != null ? src.Torneo.Nombre : null));

            CreateMap<Models.NivelMedalla, DTOs.NivelMedallaReadDto>();

            CreateMap<Models.AdministradorTorneo, DTOs.AdministradorTorneoReadDto>()
                .ForMember(dest => dest.NombreAdministrador, opt => opt.MapFrom(src => src.Administrador != null ? $"{src.Administrador.Usuario.Nombre} {src.Administrador.Usuario.Apellido}" : null))
                .ForMember(dest => dest.TipoRol, opt => opt.MapFrom(src => src.TipoRol != null ? src.TipoRol.Nombre : null))
                .ForMember(dest => dest.NombreTorneo, opt => opt.MapFrom(src => src.Torneo != null ? src.Torneo.Nombre : null));

            CreateMap<Models.EstudianteMedalla, DTOs.EstudianteMedallaReadDto>()
                .ForMember(dest => dest.NombreMedalla, opt => opt.MapFrom(src => src.Medalla != null ? src.Medalla.Nombre : null))
                .ForMember(dest => dest.NombreEstudiante, opt => opt.MapFrom(src => src.Estudiante != null ? $"{src.Estudiante.Usuario.Nombre} {src.Estudiante.Usuario.Apellido}" : null));

        }
    }
}
