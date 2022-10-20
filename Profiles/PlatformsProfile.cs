using AutoMapper;
using PlatformService.Dtos;

namespace PlatformService.Profiles
{
    public class PlatformsProfile : Profile
    {
        public PlatformsProfile()
        {
            CreateMap<Models.Platform, PlatformReadDto>();
            CreateMap<PlatformCreateDto, Models.Platform>();
            CreateMap<PlatformReadDto, PlatformPublishedDto>();
            CreateMap<Models.Platform, Protos.Platform>()
                .ForMember(dest => dest.PlatformId, opt => opt.MapFrom(src => src.Id));
        }
    }
}