using blog_rest_api.Data;
using blog_rest_api.Data.Domain;
using Microsoft.EntityFrameworkCore;

namespace blog_rest_api.Services
{
    public class BlogService : IBlogService
    {

        private readonly BlogDbContext _dbContext;

        public BlogService(BlogDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<List<Blog>> GetAllAsync(string? userId = null, PaginationFilter paginationFilter = null)
        {
            var queryable = _dbContext.Blogs.AsQueryable();

            if (paginationFilter == null)
            {
                return await queryable.ToListAsync();
            }

            if (!string.IsNullOrEmpty(userId))
            {
                queryable = queryable.Where(blog => blog.UserId == userId);
            }
            var skip = (paginationFilter.PageNumber - 1) * paginationFilter.PageSize;
            return await queryable.Include(blog => blog.Tags).ThenInclude(blogTag => blogTag.Tag).Skip(skip).Take(paginationFilter.PageSize).ToListAsync();
        }


        public async Task<Blog> GetByIdAsync(Guid blogId) => await _dbContext.Blogs.SingleOrDefaultAsync(x => x.Id == blogId);

        public async Task<bool> CreateBlogAsync(Blog blog)
        {
            await AddNewTags(blog);
            await _dbContext.Blogs.AddAsync(blog);
            var created = await _dbContext.SaveChangesAsync();
            return created > 0;
        }

        private async Task AddNewTags(Blog blog)
        {
            foreach (var tag in blog.Tags)
            {
                var isAlreadyExist = await _dbContext.Tags.SingleOrDefaultAsync(x => x.Name == tag.TagId);

                if (isAlreadyExist != null)
                    continue;

                var newTag = new Tag
                {
                    Name = tag.TagId,
                    UserId = blog.UserId,
                    CreatedAt = DateTime.Now.ToLocalTime(),
                    UpdatedAt = DateTime.Now.ToLocalTime(),
                };
                await _dbContext.Tags.AddAsync(newTag);
            }
        }

        public async Task<bool> UpdateBlogAsync(Blog blogToUpdate)
        {
            _dbContext.Blogs.Update(blogToUpdate);
            var updated = await _dbContext.SaveChangesAsync();
            return updated > 0;
        }

        public async Task<bool> DeleteBlogAsync(Guid id)
        {
            var blog = await GetByIdAsync(id);

            if (blog == null)
                return false;


            _dbContext.Blogs.Remove(blog);
            var deleted = await _dbContext.SaveChangesAsync();

            return deleted > 0;
        }

        public async Task<bool> UserOwnsPostAsync(Guid postId, string userId)
        {
            var blog = await _dbContext.Blogs.AsNoTracking().SingleOrDefaultAsync(x => x.Id == postId);
            if (blog == null)
                return false;

            if (blog.UserId != userId)
                return false;

            return true;
        }

        public async Task<Tag> GetTagByIdAsync(string tagId) => await _dbContext.Tags.SingleOrDefaultAsync(x => x.Name == tagId);
        public async Task<List<Tag>> GetAllTagsAsync()
        {
            return await _dbContext.Tags.AsNoTracking().ToListAsync();
        }



        public async Task<bool> CreateSingleTagAsync(Tag tag)
        {
            var isAlreadyExist = await _dbContext.BlogTags.SingleOrDefaultAsync(x => x.TagId == tag.Name);

            if (isAlreadyExist != null)
                return false;

            await _dbContext.Tags.AddAsync(tag);
            var created = await _dbContext.SaveChangesAsync();
            return created > 0;

        }

        public async Task<bool> DeleteTagAsync(string tagId)
        {
            var tag = await GetTagByIdAsync(tagId);
            if (tag == null) return false;

            _dbContext.Tags.Remove(tag);
            var deleted = await _dbContext.SaveChangesAsync();

            return deleted > 0;
        }
    }
}
