using AutoMapper;
using PlataformaIntegral.API.DTOs;
using PlataformaIntegral.API.Models;

namespace PlataformaIntegral.API.Profiles
{
    public class CategoriaProfile : Profile
    {
        public CategoriaProfile()
        {
            //CREATE DTOs:
            CreateMap<CategoriaCreateDto, Categoria>()
                .ForMember(dest => dest.IdCategoria, opt => opt.Ignore())
                .ForMember(dest => dest.SuperCategoria, opt => opt.Ignore())
                .ForMember(dest => dest.SubCategorias, opt => opt.Ignore())
                .ForMember(dest => dest.Cursos, opt => opt.Ignore())
                .ForMember(dest => dest.Estudiantes, opt => opt.Ignore())
                .ForMember(dest => dest.Profesores, opt => opt.Ignore())
                .ForMember(dest => dest.Torneos, opt => opt.Ignore());

            //READ DTOs:
            CreateMap<Categoria, CategoriaReadDto>()
                .ForMember(dest => dest.NombreSuperCategoria, opt => opt.MapFrom(src => src.SuperCategoria != null ? src.SuperCategoria.Nombre : null));
        }
    }
