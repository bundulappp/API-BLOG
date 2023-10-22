using AutoMapper;
using blog_rest_api.Contracts.V1.Request;
using blog_rest_api.Contracts.V1.Responses;
using blog_rest_api.Domain;

namespace blog_rest_api.MappingProfiles
{
    public class DomainToResponseProfile : Profile
    {
        public DomainToResponseProfile()
        {
            CreateMap<Blog, CreateBlogResponse>()
                .ForMember(dest => dest.Tags, opt => opt.MapFrom(src => src.BlogTags.Select(x => new TagResponse { Name = x.Tag.Name })));
            CreateMap<Tag, TagResponse>();
        }
    }

    public class RequestToDomain : Profile
    {
        public RequestToDomain()
        {
            CreateMap<CreateBlogRequest, Blog>()


        }
    }
}
