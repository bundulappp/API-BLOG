using Models.Domain;
using Models.Interfaces;
namespace Logic.Services
{
    public class CommentService : ICommentService
    {
        private readonly ICommentRepository _commentRepository;
        private readonly IBlogRepository _blogRepository;
        public CommentService(ICommentRepository commentRepository, IBlogRepository blogRepository)
        {
            _commentRepository = commentRepository;
            _blogRepository = blogRepository;
        }
        public async Task<IEnumerable<Comment>> GetAllBlogsCommentAsnyc(string blogId, PaginationFilter? paginationFilter = null)
        {
            var isBlogExist = await _blogRepository.GetById(blogId);
            if (isBlogExist == null)
            {
                throw new KeyNotFoundException($"Blog is not found with id: {blogId}");
            }
            return await _commentRepository.GetAllBlogsCommentAsync(blogId, paginationFilter);
        }

        public async Task<Comment> Get(string commentId) => await _commentRepository.GetById(commentId);
        public async Task<bool> CreateCommentAsnyc(Comment comment)
        {
            return await _commentRepository.Insert(comment);
        }

        public async Task<bool> DeleteCommentAsync(string commentId)
        {
            var comment = await _commentRepository.GetById(commentId);

            if (comment == null)
                return false;

            return await _commentRepository.Delete(comment);
        }

        public async Task<bool> UserOwnsComment(string commentId, string userId)
        {
            var comment = await _commentRepository.GetById(commentId);

            if (comment == null)
                return false;

            return comment?.UserId == userId;
        }

        public async Task<bool> UpdateCommentAsync(Comment comment)
        {
            return await _commentRepository.Update(comment);
        }
    }
}
