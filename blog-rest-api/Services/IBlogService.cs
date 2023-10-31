using blog_rest_api.Domain;

namespace blog_rest_api.Services
{
    public interface IBlogService
    {
        Task<List<Blog>> GetAllAsync(PaginationFilter paginationFilter = null);
        Task<Blog> GetByIdAsync(Guid id);
        Task<bool> CreateBlogAsync(Blog blog);
        Task<bool> UpdateBlogAsync(Blog blog);
        Task<bool> DeleteBlogAsync(Guid id);
        Task<bool> UserOwnsPostAsync(Guid postId, string userId);
        Task<Tag> GetTagByIdAsync(string tagId);
        Task<List<Tag>> GetAllTagsAsync();
        Task<bool> CreateTagAsync(Tag tag);
        Task<bool> DeleteTagAsync(string tagId);
    }
}
