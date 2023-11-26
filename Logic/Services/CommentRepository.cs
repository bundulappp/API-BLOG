﻿using Data.Data;
using Microsoft.AspNetCore.Http;
using Models.Domain;
using Models.Interfaces;

namespace Logic.Services
{
    public class CommentRepository : Repository<Comment>, ICommentRepository
    {
        public CommentRepository(IHttpContextAccessor httpContextAccessor, BlogDbContext dbContext) : base(httpContextAccessor, dbContext)
        {
        }
    }
}
