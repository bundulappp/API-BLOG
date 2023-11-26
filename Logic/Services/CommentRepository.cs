using Data.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Models.Domain;
using Models.Interfaces;

namespace Logic.Services
{
    public class CommentRepository : Repository<Comment>, ICommentRepository
    {
        public CommentRepository(IHttpContextAccessor httpContextAccessor, BlogDbContext dbContext) : base(httpContextAccessor, dbContext)
        {
        }

        public async Task<IEnumerable<Comment>> GetAllBlogsCommentAsync(string blogId)
        {
            var queryable = _dbContext.Set<Comment>().AsQueryable();

            if (!string.IsNullOrEmpty(blogId))
            {
                queryable = queryable.Where(comment => comment.BlogId == blogId);
            }

            return await queryable.ToListAsync();
        }
    }
}
