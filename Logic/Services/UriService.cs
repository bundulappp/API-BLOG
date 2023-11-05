using Microsoft.AspNetCore.WebUtilities;
using Models.Contracts.V1;
using Models.Contracts.V1.Requests.Queries;
using Models.Interfaces;

namespace Logic.Services
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
