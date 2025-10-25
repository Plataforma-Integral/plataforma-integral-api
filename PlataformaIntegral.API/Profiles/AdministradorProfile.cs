using AutoMapper;

namespace PlataformaIntegral.API.Profiles
{
    public class AdministradorProfile : Profile
    {
        public AdministradorProfile()
        {
            //CREATE DTOs:
            CreateMap<DTOs.AdministradorCreateDto, Models.Administrador>()
                .ForMember(dest => dest.IdUsuario, opt => opt.Ignore())
                .ForMember(dest => dest.AdministradorTorneos, opt => opt.Ignore())
                .ForMember(dest => dest.Usuario, opt => opt.Ignore());

            //READ DTOs:
            CreateMap<Models.Administrador, DTOs.AdministradorReadDto>()
                .ForMember(dest => dest.NombreUsuario, opt => opt.MapFrom(src => src.Usuario.Nombre));
        }
    }
}
