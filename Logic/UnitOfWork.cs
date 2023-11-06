using Data.Data;
using Logic.Services;
using Microsoft.AspNetCore.Http;
using Models.Domain;
using Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

    }
}
