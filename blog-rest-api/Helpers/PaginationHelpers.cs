using blog_rest_api.Contracts.V1.Requests.Queries;
using blog_rest_api.Contracts.V1.Responses;
using blog_rest_api.Domain;
using blog_rest_api.Services;

namespace blog_rest_api.Helpers
{
    public class PaginationHelpers
    {
        internal static PagedResponse<T> CreatePaginatedResponse<T>(IUriService uriService, PaginationFilter paginationFilter, List<T> response)
        {
            var nextPage = paginationFilter.PageNumber >= 1
               ? uriService.GetAllBlogUri(new PaginationQuery(paginationFilter.PageNumber + 1, paginationFilter.PageSize)).ToString() : null;

            var previousPage = paginationFilter.PageNumber - 1 >= 1
                ? uriService.GetAllBlogUri(new PaginationQuery(paginationFilter.PageNumber - 1, paginationFilter.PageSize)).ToString() : null;

            return new PagedResponse<T>
            {
                Data = response,
                PageNumber = paginationFilter.PageNumber <= 1 ? paginationFilter.PageNumber : (int?)null,
                PageSize = paginationFilter.PageSize >= 1 ? paginationFilter.PageSize : (int?)null,
                NextPage = response.Any() ? nextPage : null,
                PreviousPage = previousPage
            };
        }
    }
}
