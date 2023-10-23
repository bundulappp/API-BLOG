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
            CreateMap<Blog, BlogResponse>()
                .ForMember(dest => dest.Tags, opt => opt.MapFrom(src => src.Tags.Select(x => new CreateTagResponse { Name = x.TagId })));
            CreateMap<Tag, CreateTagResponse>();
        }
    }

    public class RequestToDomain : Profile
    {
        public RequestToDomain()
        {
            CreateMap<CreateBlogRequest, Blog>();


        }
    }
}
