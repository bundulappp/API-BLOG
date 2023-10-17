using blog_rest_api.Domain;

namespace blog_rest_api.Services
{
    public class BlogService : IBlogService
    {
        private List<Blog> _blogs;
        public BlogService()
        {
            _blogs = new List<Blog>();
            for (var i = 0; i < 5; i++)
            {
                _blogs.Add(new Blog
                {
                    Id = Guid.NewGuid(),
                    Name = $"Blog name {i}"
                });
            }
        }
        public List<Blog> GetAll() => _blogs;


        public Blog GetById(Guid blogId) => _blogs.SingleOrDefault(b => b.Id == blogId);

    }
}
