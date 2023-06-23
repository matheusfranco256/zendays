using AutoMapper;
using ZenDays.Core.Models;
using ZenDays.Domain.Entities;

namespace ZenDays.Service.DTO.Config
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Base, BaseDTO>().ReverseMap();
            CreateMap<Usuario, UsuarioDTO>().ReverseMap();
            CreateMap<RegisterUserViewModel, UsuarioDTO>().ReverseMap();
            CreateMap<UpdateUserViewModel, UsuarioDTO>().ReverseMap();

            CreateMap<Departamento, DepartamentoDTO>().ReverseMap();
            CreateMap<RegisterDepartamentoViewModel, DepartamentoDTO>().ReverseMap();
            CreateMap<UpdateDepartamentoViewModel, DepartamentoDTO>().ReverseMap();

            CreateMap<Ferias, FeriasDTO>().ReverseMap();
            CreateMap<RegisterFeriasViewModel, FeriasDTO>().ReverseMap();
            CreateMap<UpdateFeriasViewModel, FeriasDTO>().ReverseMap();
        }
    }
}
