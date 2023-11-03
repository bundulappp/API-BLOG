using blog_rest_api.Contracts.V1;
using blog_rest_api.Contracts.V1.Requests.Queries;
using Microsoft.AspNetCore.WebUtilities;

namespace blog_rest_api.Services
{
    public class UriService : IUriService
    {
        private readonly string _baseUri;
        public UriService(string baseUri)
        {
            _baseUri = baseUri;
        }
        public Uri GetBlogUri(string blogId)
        {
            return new Uri(_baseUri + ApiRoutes.Blogs.Get.Replace("{blogId}", blogId));
        }
        public Uri GetAllBlogUri(PaginationQuery paginationQuery = null)
        {
            var uri = new Uri(_baseUri);

            if (paginationQuery == null)
            {
                return uri;
            }

            var modifiedUri = QueryHelpers.AddQueryString(_baseUri, "pageNumber", paginationQuery.PageNumber.ToString());
            modifiedUri = QueryHelpers.AddQueryString(modifiedUri, "pageSize", paginationQuery.PageSize.ToString());

            return new Uri(modifiedUri);
        }

    }
}
