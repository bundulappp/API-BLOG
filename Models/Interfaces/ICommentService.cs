using Models.Domain;

namespace Models.Interfaces
{
    public interface ICommentService
    {
        Task<IEnumerable<Comment>> GetAllBlogsCommentAsnyc(string blogId, PaginationFilter? paginationFilter = null);
        Task<bool> CreateCommentAsnyc(Comment comment);
        Task<bool> DeleteCommentAsync(string commentId);



    }
}
