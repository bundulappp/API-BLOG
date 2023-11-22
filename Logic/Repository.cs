using Data.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Models.Domain;
using Models.Interfaces;

namespace Logic
{
    public abstract class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        protected readonly BlogDbContext _dbContext;
        protected IHttpContextAccessor _httpContextAccessor;


        public Repository(IHttpContextAccessor httpContextAccessor, BlogDbContext dbContext)
        {
            _dbContext = dbContext;
            _httpContextAccessor = httpContextAccessor;
        }

        public virtual async Task<IEnumerable<TEntity>> GetAll(PaginationFilter? paginationFilter = null)
        {
            var queryable = _dbContext.Set<TEntity>().AsQueryable();

            if (paginationFilter != null)
            {
                var skip = (paginationFilter.PageNumber - 1) * paginationFilter.PageSize;
                queryable = queryable.Skip(skip).Take(paginationFilter.PageSize);
            }

            return await queryable.ToListAsync();
        }

        public virtual async Task<TEntity> GetById(string id)
        {
            return await _dbContext.Set<TEntity>().FindAsync(id);
        }

        public virtual async Task<bool> Insert(TEntity entity)
        {
            _dbContext.Set<TEntity>().Add(entity);
            var result = await _dbContext.SaveChangesAsync();
            return result > 0;
        }

        public virtual async Task<bool> Update(TEntity entity)
        {
            _dbContext.Set<TEntity>().Update(entity);
            var result = await _dbContext.SaveChangesAsync();
            return result > 0;
        }

        public virtual async Task<bool> Delete(TEntity entity)
        {
            _dbContext.Set<TEntity>().Remove(entity);
            var result = await _dbContext.SaveChangesAsync();
            return result > 0;
        }
    }
}
