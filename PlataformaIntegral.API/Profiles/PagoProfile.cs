using AutoMapper;
using PlataformaIntegral.API.DTOs;

namespace PlataformaIntegral.API.Profiles
{
    public class PagoProfile : Profile
    {
        public PagoProfile()
        {
            // CREATE DTOs:
            CreateMap<DTOs.PagoCreateDto, Models.Pago>()
                .ForMember(dest => dest.IdPago, opt => opt.Ignore())
                .ForMember(dest => dest.IdRecibo, opt => opt.Ignore())
                .ForMember(dest => dest.EstadoPago, opt => opt.Ignore())
                .ForMember(dest => dest.MetodoPago, opt => opt.Ignore())
                .ForMember(dest => dest.Producto, opt => opt.Ignore())
                .ForMember(dest => dest.Recibo, opt => opt.Ignore())
                .ForMember(dest => dest.TipoMoneda, opt => opt.Ignore())
                .ForMember(dest => dest.Usuario, opt => opt.Ignore());

            CreateMap<DTOs.ReciboCreateDto, Models.Recibo>()
                .ForMember(dest => dest.IdRecibo, opt => opt.Ignore())
                .ForMember(dest => dest.IdUsuario, opt => opt.Ignore())
                .ForMember(dest => dest.FechaEmision, opt => opt.Ignore())
                .ForMember(dest => dest.EstadoPago, opt => opt.Ignore())
                .ForMember(dest => dest.Usuario, opt => opt.Ignore())
                .ForMember(dest => dest.Pagos, opt => opt.Ignore());

            CreateMap<DTOs.TipoMonedaCreateDto, Models.TipoMoneda>()
                .ForMember(dest => dest.IdTipoMoneda, opt => opt.Ignore());

            CreateMap<DTOs.MetodoPagoCreateDto, Models.MetodoPago>()
                .ForMember(dest => dest.IdMetodoPago, opt => opt.Ignore());

            CreateMap<DTOs.EstadoPagoCreateDto, Models.EstadoPago>()
                .ForMember(dest => dest.IdEstadoPago, opt => opt.Ignore());

            //READ DTOs:}
            CreateMap<Models.Pago, DTOs.PagoReadDto>()
                .ForMember(dest => dest.NombreEstadoPago, opt => opt.MapFrom(src => src.EstadoPago.Nombre))
                .ForMember(dest => dest.NombreMetodoPago, opt => opt.MapFrom(src => src.MetodoPago.Nombre))
                .ForMember(dest => dest.NombreProducto, opt => opt.MapFrom(src => src.Producto.Nombre))
                .ForMember(dest => dest.NumeroRecibo, opt => opt.MapFrom(src => src.Recibo != null ? src.Recibo.NumeroRecibo : null))
                .ForMember(dest => dest.NombreTipoMoneda, opt => opt.MapFrom(src => src.TipoMoneda.Nombre))
                .ForMember(dest => dest.NombreUsuario, opt => opt.MapFrom(src => src.Usuario.Nombre));

            CreateMap<Models.Recibo, DTOs.ReciboReadDto>()
                .ForMember(dest => dest.NombreEstadoPago, opt => opt.MapFrom(src => src.EstadoPago != null ? src.EstadoPago.Nombre : null))
                .ForMember(dest => dest.NombreUsuario, opt => opt.MapFrom(src => src.Usuario != null ? src.Usuario.Nombre : null));

            CreateMap<Models.TipoMoneda, DTOs.TipoMonedaReadDto>();

            CreateMap<Models.MetodoPago, DTOs.MetodoPagoReadDto>();

            CreateMap<Models.EstadoPago, DTOs.EstadoPagoReadDto>();

        }
    }
}
