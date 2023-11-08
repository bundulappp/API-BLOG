using Data.Data;
using Microsoft.AspNetCore.Http;
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

        public virtual IEnumerable<TEntity> GetAll(PaginationFilter? paginationFilter = null)
        {
            var queryable = _dbContext.Set<TEntity>().AsQueryable();

            if (paginationFilter != null)
            {
                var skip = (paginationFilter.PageNumber - 1) * paginationFilter.PageSize;
                queryable = queryable.Skip(skip).Take(paginationFilter.PageSize);
            }

            return queryable.ToList();
        }

        public virtual TEntity GetById(string id)
        {
            return _dbContext.Set<TEntity>().Find(id);
        }

        public virtual bool Insert(TEntity entity)
        {
            _dbContext.Set<TEntity>().Add(entity);
            return _dbContext.SaveChanges() > 0;
        }

        public virtual bool Update(TEntity entity)
        {
            _dbContext.Set<TEntity>().Update(entity);
            return _dbContext.SaveChanges() > 0;
        }

        public virtual bool Delete(TEntity entity)
        {
            _dbContext.Set<TEntity>().Remove(entity);
            return _dbContext.SaveChanges() > 0;
        }
    }
}
