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

            //READ DTOs:
            CreateMap<Curso, CursoReadDto>()
                .ForMember(dest => dest.NombreProducto, opt => opt.MapFrom(src => src.Producto.Nombre));
        }
    }
}
