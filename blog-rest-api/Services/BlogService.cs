﻿using blog_rest_api.Data;
using blog_rest_api.Domain;
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

        public async Task<List<Blog>> GetAllAsync() => await _dbContext.Blogs.ToListAsync();


        public async Task<Blog> GetByIdAsync(Guid blogId) => await _dbContext.Blogs.SingleOrDefaultAsync(x => x.Id == blogId);

        public async Task<bool> CreateBlogAsync(Blog blog)
        {
            await _dbContext.Blogs.AddAsync(blog);
            var created = await _dbContext.SaveChangesAsync();
            return created > 0;
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

    }
}
