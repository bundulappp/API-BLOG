using Models.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Interfaces
{
    public interface IRepository<TEntity>
    {
        public IEnumerable<TEntity> GetAll(string? userId = null, PaginationFilter? paginationFilter = null);
        public TEntity GetById(string id);
        public bool Insert(TEntity entity);
        public bool Update(TEntity entity);
        public bool Delete(TEntity entity);

    }
}
