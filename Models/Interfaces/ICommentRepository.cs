using Models.Domain;

namespace Models.Interfaces
{
    public interface ICommentRepository : IRepository<Comment>
    {
        public Task<IEnumerable<Comment>> GetAllBlogsCommentAsync(string blogId);
    }
}
