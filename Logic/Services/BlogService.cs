using Models.Domain;
using Models.Interfaces;

namespace Logic.Services
{
    public class BlogService : IBlogService
    {

        private readonly IUnitOfWork _unitOfWork;
        public BlogService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<Blog>> GetAllAsync(string? userId = null, PaginationFilter? paginationFilter = null)
            => await _unitOfWork.BlogRepository.GetAll(paginationFilter, userId);



        public async Task<Blog> GetByIdAsync(string blogId) => await _unitOfWork.BlogRepository.GetById(blogId);

        public async Task<bool> CreateBlogAsync(Blog blog)
        {
            await AddNewTags(blog);

            return await _unitOfWork.BlogRepository.Insert(blog);
        }

        private async Task AddNewTags(Blog blog)
        {
            var tasks = blog.Tags.Select(async tag =>
             {

                 {
                     var isAlreadyExist = await _unitOfWork.TagRepository.GetById(tag.TagId);

                     if (isAlreadyExist == null)
                     {
                         var newTag = new Tag
                         {
                             Name = tag.TagId,
                             UserId = blog.UserId,
                             CreatedAt = DateTime.Now.ToLocalTime(),
                             UpdatedAt = DateTime.Now.ToLocalTime(),
                         };

                         await _unitOfWork.TagRepository.Insert(newTag);
                     }

                 }
             });
            await Task.WhenAll(tasks);
        }

        public async Task<bool> UpdateBlogAsync(Blog blogToUpdate)
        {
            var isAlreadyExist = await _unitOfWork.BlogRepository.GetById(blogToUpdate.Id.ToString());

            if (isAlreadyExist == null)
                return false;

            return await _unitOfWork.BlogRepository.Update(blogToUpdate);
        }

        public async Task<bool> DeleteBlogAsync(string id)
        {
            var blog = await _unitOfWork.BlogRepository.GetById(id);

            if (blog == null)
                return false;

            return await _unitOfWork.BlogRepository.Delete(blog);
        }

        public async Task<bool> UserOwnsBlogAsync(string blogId, string userId)
        {
            var blog = await _unitOfWork.BlogRepository.GetById(blogId);
            if (blog == null)
                return false;

            if (blog.UserId != userId)
                return false;

            return true;
        }
        public async Task<Tag> GetTagByIdAsync(string tagId) => await _unitOfWork.TagRepository.GetById(tagId);
        public async Task<IEnumerable<Tag>> GetAllTagsAsync(string? userId = null, PaginationFilter? paginationFilter = null)
             => await _unitOfWork.TagRepository.GetAll(paginationFilter);

        public async Task<bool> CreateSingleTagAsync(Tag tag)
        {
            var isAlreadyExist = await _unitOfWork.TagRepository.GetById(tag.Name);

            if (isAlreadyExist != null)
                return false;

            return await _unitOfWork.TagRepository.Insert(tag);
        }

        public async Task<bool> DeleteTagAsync(string tagId)
        {
            var tag = await _unitOfWork.TagRepository.GetById(tagId);
            if (tag == null)
                return false;

            return await _unitOfWork.TagRepository.Delete(tag);
        }
    }
}
