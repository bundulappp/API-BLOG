using Data.Data;
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

        public async Task<IEnumerable<Blog>> GetAllAsync(string? userId = null, PaginationFilter? paginationFilter = null)
            => await _blogRepository.GetAll(paginationFilter, userId);



        public async Task<Blog> GetByIdAsync(string blogId) => await _blogRepository.GetById(blogId);

        public async Task<bool> CreateBlogAsync(Blog blog)
        {
            await AddNewTags(blog);

            return await _blogRepository.Insert(blog);
        }

        private async Task AddNewTags(Blog blog)
        {
            var tasks = blog.Tags.Select(async tag =>
             {

                 {
                     var isAlreadyExist = await _tagRepository.GetById(tag.TagId);

                     if (isAlreadyExist == null)
                     {
                         var newTag = new Tag
                         {
                             Name = tag.TagId,
                             UserId = blog.UserId,
                             CreatedAt = DateTime.Now.ToLocalTime(),
                             UpdatedAt = DateTime.Now.ToLocalTime(),
                         };

                         await _tagRepository.Insert(newTag);
                     }

                 }
             });
            await Task.WhenAll(tasks);
        }

        public async Task<bool> UpdateBlogAsync(Blog blogToUpdate)
        {
            var isAlreadyExist = await _blogRepository.GetById(blogToUpdate.Id.ToString());

            if (isAlreadyExist == null)
                return false;

            return await _blogRepository.Update(blogToUpdate);
        }

        public async Task<bool> DeleteBlogAsync(string id)
        {
            var blog = await _blogRepository.GetById(id);

            if (blog == null)
                return false;

            return await _blogRepository.Delete(blog);
        }

        public async Task<bool> UserOwnsBlogAsync(string blogId, string userId)
        {
            var blog = await _blogRepository.GetById(blogId);
            if (blog == null)
                return false;

            if (blog.UserId != userId)
                return false;

            return true;
        }

        public async Task<Tag> GetTagByIdAsync(string tagId) => await _tagRepository.GetById(tagId);
        public async Task<IEnumerable<Tag>> GetAllTagsAsync(string? userId = null, PaginationFilter? paginationFilter = null)
             => await _tagRepository.GetAll(paginationFilter);

        public async Task<bool> CreateSingleTagAsync(Tag tag)
        {
            var isAlreadyExist = await _tagRepository.GetById(tag.Name);

            if (isAlreadyExist != null)
                return false;

            return await _tagRepository.Insert(tag);
        }

        public async Task<bool> DeleteTagAsync(string tagId)
        {
            var tag = await _tagRepository.GetById(tagId);
            if (tag == null)
                return false;

            return await _tagRepository.Delete(tag);
        }
    }
}
