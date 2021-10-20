using AutoMapper;
using ChatApplication.Core.DTO;
using ChatApplication.Core.ErrorHandling;
using ChatApplication.WebAPI.Models;

namespace ChatApplication.WebAPI.AutoMapper
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
            CreateMap<RefreshTokenWrapper, RefreshTokenDTO>().ReverseMap();
        }

        private void MapMessage()
        {
            CreateMap<MessagePostWrapper, MessageDTO>();
        }

        private void MapUser()
        {
            CreateMap<RegisterUserPostWrapper, CredentialsDTO>();
            CreateMap<RegisterUserPostWrapper, UserDTO>().ForMember(dest => dest.Credentials, opt => opt.MapFrom(src => src));
            CreateMap<AuthenticateCredentialsPostWrapper, CredentialsDTO>();

            CreateMap<ResultMessage<bool>, ResultMessage<RegisterUserResponseWrapper>>();
            CreateMap<bool, RegisterUserResponseWrapper>().ForCtorParam("isRegistered", conf => conf.MapFrom(x => x));

            CreateMap<ResultMessage<string>, ResultMessage<AuthenticateCredentialsResponseWrapper>>();
            CreateMap<string, AuthenticateCredentialsResponseWrapper>().ForCtorParam("otpApiKey", conf => conf.MapFrom(x => x));

            CreateMap<OutputUserDTO, UserWrapper>();
        }
    }
}
