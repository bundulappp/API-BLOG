using Models.Domain;

namespace Models.Interfaces
{
    public interface ICommentService
    {
        Task<IEnumerable<Comment>> GetAllBlogsCommentAsnyc(string blogId);
        Task<bool> CreateCommentAsnyc(Comment comment);
        Task<bool> DeleteCommentAsync(string commentId);



    }
}
