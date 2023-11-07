using Data.Data;
using Logic.Services;
using Microsoft.AspNetCore.Http;
using Models.Interfaces;

namespace Logic
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly BlogDbContext _dbContext;

        public UnitOfWork(IHttpContextAccessor httpContextAccessor, BlogDbContext dbContext)
        {
            _dbContext = dbContext;
            _httpContextAccessor = httpContextAccessor;
        }

        public IBlogRepository BlogRepository => new BlogRepository(_httpContextAccessor, _dbContext);
        public ITagRepository TagRepository => new TagRepository(_httpContextAccessor, _dbContext);

    }
}
