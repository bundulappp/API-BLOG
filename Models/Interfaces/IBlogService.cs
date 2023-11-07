using Models.Domain;

namespace Models.Interfaces
{
    public interface IBlogService
    {
        Task<List<Blog>> GetAllAsync(string? userId = null, PaginationFilter? paginationFilter = null);
        Task<Blog> GetByIdAsync(string blogId);
        Task<bool> CreateBlogAsync(Blog blog);
        Task<bool> UpdateBlogAsync(Blog blog);
        Task<bool> DeleteBlogAsync(string blogId);
        Task<bool> UserOwnsBlogAsync(string blogId, string userId);
        Task<Tag> GetTagByIdAsync(string tagId);
        Task<List<Tag>> GetAllTagsAsync();
        Task<bool> CreateSingleTagAsync(Tag tag);
        Task<bool> DeleteTagAsync(string tagId);
    }
}
