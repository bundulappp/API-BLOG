using Data.Data;
using Microsoft.AspNetCore.Http;
using Models.Domain;
using Models.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Logic.Services
{
    public class BlogRepository : Repository<Blog>, IBlogRepository
    {
        public BlogRepository(IHttpContextAccessor httpContextAccessor, BlogDbContext dbContext) : base(httpContextAccessor, dbContext)
        {
        }
    }
}
