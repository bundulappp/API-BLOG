using Data.Data;
using Microsoft.AspNetCore.Http;
using Models.Domain;
using Models.Interfaces;

namespace Logic.Repositories
{
    public class TagRepository : Repository<Tag>, ITagRepository
    {
        public TagRepository(IHttpContextAccessor httpContextAccessor, BlogDbContext dbContext) : base(httpContextAccessor, dbContext)
        {
        }
    }
}
