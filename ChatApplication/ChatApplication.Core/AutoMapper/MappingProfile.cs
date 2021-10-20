using AutoMapper;
using ChatApplication.Core.DTO;
using ChatApplication.Core.Entities;
using System;

namespace ChatApplication.Core.AutoMapper
{
    internal class MappingProfile : Profile
    {
        public MappingProfile()
        {
            MapUser();
            MapMessage();
            MapRefreshToken();
        }

        private void MapRefreshToken()
        {
            CreateMap<RefreshTokenDTO, RefreshToken>().ReverseMap();
        }

        private void MapMessage()
        {
            CreateMap<MessageDTO, Message>();
            CreateMap<Message, OutputMessageDTO>().ForMember(dest => dest.SentOn, opt => opt.MapFrom(src => DateTime.SpecifyKind(src.SentOn, DateTimeKind.Utc)));
        }

        private void MapUser()
        {
            CreateMap<UserDTO, User>().ForMember(dest => dest.Password, opt => opt.Ignore())
                                      .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.Credentials.Username));
            CreateMap<User, OutputUserDTO>();
        }
    }
}
