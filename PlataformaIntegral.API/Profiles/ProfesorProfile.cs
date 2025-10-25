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


            //READ DTOs:
            CreateMap<Profesor, ProfesorReadDto>()
                .ForMember(dest => dest.NombreUsuario, opt => opt.MapFrom(src => src.Usuario.Nombre));
        }
    }
}
