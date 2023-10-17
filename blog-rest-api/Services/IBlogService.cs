using blog_rest_api.Domain;

namespace blog_rest_api.Services
{
    public interface IBlogService
    {
        public List<Blog> GetAll();
        public Blog GetById(Guid id);
    }
}
