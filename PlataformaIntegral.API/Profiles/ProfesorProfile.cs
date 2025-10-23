using AutoMapper;
using PlataformaIntegral.API.DTOs;
using PlataformaIntegral.API.Models;

namespace PlataformaIntegral.API.Profiles
{
    public class ProfesorProfile : Profile
    {
        public ProfesorProfile() 
        {
            CreateMap<ProfesorCreateDto, Profesor>();
            CreateMap<Profesor, ProfesorReadDto>()
                .ForMember(dest => dest.NombreUsuario, opt => opt.MapFrom(src => src.Usuario.Nombre));
        }
    }
}
