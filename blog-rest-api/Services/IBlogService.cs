using blog_rest_api.Domain;

namespace blog_rest_api.Services
{
    public interface IBlogService
    {
        Task<List<Blog>> GetAllAsync();
        Task<Blog> GetByIdAsync(Guid id);
        Task<bool> CreateBlogAsync(Blog blog);
        Task<bool> UpdateBlogAsync(Blog blog);
        Task<bool> DeleteBlogAsync(Guid id);
    }
}
