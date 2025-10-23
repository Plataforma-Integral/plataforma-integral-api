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
            CreateMap<EstudianteCreateDto, Estudiante>();

            //READ DTOs:
            CreateMap<Estudiante, EstudianteReadDto>()
                .ForMember(dest => dest.NombreUsuario, opt => opt.MapFrom(src => src.Usuario.Nombre));
        }
    }
}
