using Models.Domain;

namespace Models.Interfaces
{
    public interface IRepository<TEntity>
    {
        public Task<IEnumerable<TEntity>> GetAll(PaginationFilter? paginationFilter = null);
        public Task<TEntity> GetById(string id);
        public Task<bool> Insert(TEntity entity);
        public Task<bool> Update(TEntity entity);
        public Task<bool> Delete(TEntity entity);

    }
}
