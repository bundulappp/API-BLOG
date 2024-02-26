using Models.Domain;
using Models.Interfaces;
namespace Logic.Services
{
    public class CommentService : ICommentService
    {
        private readonly IUnitOfWork _unitOfWork;
        public CommentService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<IEnumerable<Comment>> GetAllBlogsCommentAsnyc(string blogId, PaginationFilter? paginationFilter = null)
        {
            var isBlogExist = await _unitOfWork.BlogRepository.GetById(blogId);
            if (isBlogExist == null)
            {
                throw new KeyNotFoundException($"Blog is not found with id: {blogId}");
            }
            return await _unitOfWork.CommentRepository.GetAllBlogsCommentAsync(blogId, paginationFilter);
        }

        public async Task<Comment> Get(string commentId) => await _unitOfWork.CommentRepository.GetById(commentId);
        public async Task<bool> CreateCommentAsnyc(Comment comment)
        {
            return await _unitOfWork.CommentRepository.Insert(comment);
        }

        public async Task<bool> DeleteCommentAsync(string commentId)
        {
            var comment = await _unitOfWork.CommentRepository.GetById(commentId);

            if (comment == null)
                return false;

            return await _unitOfWork.CommentRepository.Delete(comment);
        }

        public async Task<bool> UserOwnsComment(string commentId, string userId)
        {
            var comment = await _unitOfWork.CommentRepository.GetById(commentId);

            if (comment == null)
                return false;

            return comment?.UserId == userId;
        }

        public async Task<bool> UpdateCommentAsync(Comment comment)
        {
            return await _unitOfWork.CommentRepository.Update(comment);
        }
    }
}
