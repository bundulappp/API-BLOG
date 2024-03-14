using AutoMapper;
using Models.Contracts.V1.Requests;
using Models.Contracts.V1.Requests.Queries;
using Models.Domain;

namespace blog_rest_api.MappingProfiles
{
    public class RequestToDomainProfile : Profile
    {
        public RequestToDomainProfile()
        {
            CreateMap<CreateTagRequest, Tag>()
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.Now.ToLocalTime()))
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => DateTime.Now.ToLocalTime()));

            CreateMap<PaginationQuery, PaginationFilter>();

            CreateMap<CreateCommentRequest, Comment>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.NewGuid().ToString()))
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.Now.ToLocalTime()))
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => DateTime.Now.ToLocalTime()))
                .ForMember(dest => dest.LikesCounter, opt => opt.MapFrom(src => 0));

            CreateMap<ReplyCommentRequest, Comment>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => Guid.NewGuid().ToString()))
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.Now.ToLocalTime()))
                .ForMember(dest => dest.UpdatedAt, opt => opt.MapFrom(src => DateTime.Now.ToLocalTime()))
                .ForMember(dest => dest.LikesCounter, opt => opt.MapFrom(src => 0))
                .ForMember(dest => dest.BlogId, opt => opt.MapFrom(src => src.BlogId));

        }
    }
}
