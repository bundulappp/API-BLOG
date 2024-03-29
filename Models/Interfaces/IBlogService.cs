﻿using Models.Domain;

namespace Models.Interfaces
{
    public interface IBlogService
    {
        Task<IEnumerable<Blog>> GetAllAsync(string? userId = null, PaginationFilter? paginationFilter = null);
        Task<Blog> GetByIdAsync(string blogId);
        Task<bool> CreateBlogAsync(Blog blog);
        Task<bool> UpdateBlogAsync(Blog blog);
        Task<bool> DeleteBlogAsync(string blogId);
        Task<bool> UserOwnsBlogAsync(string blogId, string userId);
        Task<Tag> GetTagByIdAsync(string tagId);
        Task<IEnumerable<Tag>> GetAllTagsAsync(string? userId = null, PaginationFilter? paginationFilter = null);
        Task<bool> CreateSingleTagAsync(Tag tag);
        Task<bool> DeleteTagAsync(string tagId);
    }
}
