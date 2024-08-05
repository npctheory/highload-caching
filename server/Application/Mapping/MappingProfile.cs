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

            CreateMap<TokenDAO, Token>();
            CreateMap<Token, TokenDAO>();
            
            CreateMap<Token, TokenDTO>()
                .ForMember(dest => dest.token, opt => opt.MapFrom(src => src.Value));

            CreateMap<TokenDTO, Token>()
                .ForMember(dest => dest.Value, opt => opt.MapFrom(src => src.token));

            CreateMap<FriendDAO, Friend>();
            CreateMap<Friend, FriendDTO>()
                .ForMember(dest => dest.user_id, opt => opt.MapFrom(src => src.FriendId));;
        }
    }
}