using AutoMapper;
using Models.Contracts.V1.Responses;
using Models.Domain;

namespace blog_rest_api.MappingProfiles
{
    public class DomainToResponseProfile : Profile
    {
        public DomainToResponseProfile()
        {
            CreateMap<Blog, BlogResponse>()
                .ForMember(dest => dest.Tags, opt => opt.MapFrom(src => src.Tags.Select(x => new TagResponse { Name = x.TagId, UserId = x.Tag.UserId, CreatedAt = x.Tag.CreatedAt, UpdatedAt = x.Tag.UpdatedAt })));
            CreateMap<Tag, TagResponse>();
            CreateMap<Comment, CommentResponse>();
        }
    }
}
