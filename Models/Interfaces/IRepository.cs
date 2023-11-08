using Models.Domain;

namespace Models.Interfaces
{
    public interface IRepository<TEntity>
    {
        public IEnumerable<TEntity> GetAll(PaginationFilter? paginationFilter = null);
        public TEntity GetById(string id);
        public bool Insert(TEntity entity);
        public bool Update(TEntity entity);
        public bool Delete(TEntity entity);

    }
}
