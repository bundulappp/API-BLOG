using Data.Data;
using Microsoft.AspNetCore.Http;
using Models.Domain;
using Models.Interfaces;

namespace Logic.Services
{
    public class TagRepository : Repository<Tag>, ITagRepository
    {
        public TagRepository(IHttpContextAccessor httpContextAccessor, BlogDbContext dbContext) : base(httpContextAccessor, dbContext)
        {
        }
    }
}
