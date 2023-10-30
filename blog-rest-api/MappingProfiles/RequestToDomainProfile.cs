using AutoMapper;
using blog_rest_api.Contracts.V1.Requests;
using blog_rest_api.Domain;

namespace blog_rest_api.MappingProfiles
{
    public class RequestToDomainProfile : Profile
    {
        public RequestToDomainProfile()
        {
            CreateMap<CreateTagRequest, Tag>()
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.Now.ToLocalTime()))
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => DateTime.Now.ToLocalTime()));


        }
    }
}
