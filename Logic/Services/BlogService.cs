using Data.Data;
using Microsoft.EntityFrameworkCore;
using Models.Domain;
using Models.Interfaces;

namespace Logic.Services
{
    public class BlogService : IBlogService
    {

        private readonly BlogDbContext _dbContext;
        private readonly IBlogRepository _blogRepository;
        private readonly ITagRepository _tagRepository;




        public BlogService(BlogDbContext dbContext, IBlogRepository blogRepository, ITagRepository tagRepository)
        {
            _dbContext = dbContext;
            _blogRepository = blogRepository;
            _tagRepository = tagRepository;
        }

        public async Task<List<Blog>> GetAllAsync(string? userId = null, PaginationFilter? paginationFilter = null)
        {
            var blogs = _blogRepository.GetAll(paginationFilter);

            if (!string.IsNullOrEmpty(userId))
            {
                blogs = blogs.Where(b => b.UserId == userId);
            }
            return await Task.FromResult(blogs.ToList());
        }


        public async Task<Blog> GetByIdAsync(string blogId) => _blogRepository.GetById(blogId);

        public async Task<bool> CreateBlogAsync(Blog blog)
        {
            await AddNewTags(blog);

            return _blogRepository.Insert(blog);
        }

        private async Task AddNewTags(Blog blog)
        {
            foreach (var tag in blog.Tags)
            {
                var isAlreadyExist = _tagRepository.GetById(tag.TagId);

                if (isAlreadyExist != null)
                    continue;

                var newTag = new Tag
                {
                    Name = tag.TagId,
                    UserId = blog.UserId,
                    CreatedAt = DateTime.Now.ToLocalTime(),
                    UpdatedAt = DateTime.Now.ToLocalTime(),
                };

                _tagRepository.Insert(newTag);
            }
        }

        public async Task<bool> UpdateBlogAsync(Blog blogToUpdate)
        {
            var isAlreadyExist = _blogRepository.GetById(blogToUpdate.Id.ToString());

            if (isAlreadyExist == null)
                return false;

            return _blogRepository.Update(blogToUpdate);
        }

        public async Task<bool> DeleteBlogAsync(string id)
        {
            var blog = _blogRepository.GetById(id);

            if (blog == null)
                return false;

            return _blogRepository.Delete(blog);
        }

        public async Task<bool> UserOwnsBlogAsync(string blogId, string userId)
        {
            var blog = _blogRepository.GetById(blogId);
            if (blog == null)
                return false;

            if (blog.UserId != userId)
                return false;

            return true;
        }

        public async Task<Tag> GetTagByIdAsync(string tagId) => _tagRepository.GetById(tagId);
        public async Task<List<Tag>> GetAllTagsAsync()
        {
            return _tagRepository.GetAll().ToList();
        }



        public async Task<bool> CreateSingleTagAsync(Tag tag)
        {
            var isAlreadyExist = _tagRepository.GetById(tag.Name);

            if (isAlreadyExist != null)
                return false;

            return _tagRepository.Insert(tag);
        }

        public async Task<bool> DeleteTagAsync(string tagId)
        {
            var tag = _tagRepository.GetById(tagId);
            if (tag == null)
                return false;

            return _tagRepository.Delete(tag);
        }
    }
}
