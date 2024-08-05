using AutoMapper;
using Application.DTO;
using Domain.Entities;
using Application.DAO;

namespace Application.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<UserDAO, User>();
            CreateMap<User, UserDTO>();

            CreateMap<Token, TokenDAO>();

            CreateMap<Token, TokenDTO>()
                .ForMember(dest => dest.token, opt => opt.MapFrom(src => src.Value));
        }
    }
}
