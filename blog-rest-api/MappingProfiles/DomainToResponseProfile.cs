using AutoMapper;
using blog_rest_api.Contracts.V1.Responses;
using blog_rest_api.Domain;

namespace blog_rest_api.MappingProfiles
{
    public class DomainToResponseProfile : Profile
    {
        public DomainToResponseProfile()
        {
            CreateMap<Blog, BlogResponse>()
                .ForMember(dest => dest.Tags, opt => opt.MapFrom(src => src.Tags.Select(x => new TagResponse { Name = x.TagId })));
            CreateMap<Tag, TagResponse>();

        }
    }
}
