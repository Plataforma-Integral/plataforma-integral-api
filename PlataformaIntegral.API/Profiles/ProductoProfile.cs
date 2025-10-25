using AutoMapper;
using PlataformaIntegral.API.DTOs;
using PlataformaIntegral.API.Models;

namespace PlataformaIntegral.API.Profiles
{
    public class ProductoProfile : Profile
    {
        public ProductoProfile() 
        {
            //CREATE DTOs:
            CreateMap<ProductoCreateDto, Producto>()
                .ForMember(dest => dest.IdProducto, opt => opt.Ignore())
                .ForMember(dest => dest.FechaCreacion, opt => opt.Ignore())
                .ForMember(dest => dest.Pagos, opt => opt.Ignore())
                .ForMember(dest => dest.Curso, opt => opt.Ignore())
                .ForMember(dest => dest.SuscripcionTipo, opt => opt.Ignore());
            //READ DTOs:
            CreateMap<Producto, ProductoReadDto>();
        }
    }
}
