using Models.Contracts.V1.Requests.Queries;

namespace Models.Interfaces
{
    public interface IUriService
    {
        Uri GetBlogUri(string blogId);
        Uri GetAllBlogUri(PaginationQuery? paginationQuery = null);
    }
}
