using Models.Contracts.V1.Requests.Queries;

namespace Models.Interfaces
{
    public interface IUriService
    {
        Uri GetBlogUri(string blogId);
        Uri GetAllUri(PaginationQuery? paginationQuery = null);
        Uri GetCommentUri(string commentId);
    }
}
