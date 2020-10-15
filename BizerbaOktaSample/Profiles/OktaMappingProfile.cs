using AutoMapper;
using BizerbaOktaSample.Utilities.Okta;
using Okta.Sdk;

namespace BizerbaOktaSample.Profiles
{
    public class OktaMappingProfile : Profile
    {
        public OktaMappingProfile()
        {
            CreateMap<IGroup, OktaUserGroup>()
               .ForMember(dest => dest.FriendlyName,
                          opt => opt.MapFrom(src => src.Profile.Name))
               .ForMember(dest => dest.Description,
                          opt => opt.MapFrom(src => src.Profile.Description));
        }
    }
}