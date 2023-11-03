using blog_rest_api.Contracts.V1.Requests.Queries;

namespace blog_rest_api.Services
{
    public interface IUriService
    {
        Uri GetBlogUri(string blogId);
        Uri GetAllBlogUri(PaginationQuery paginationQuery = null);
    }
}
