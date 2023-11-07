using AutoMapper;
using blog_rest_api.Extensions;
using blog_rest_api.Helpers;
using Microsoft.AspNetCore.Mvc;
using Models.Contracts.V1;
using Models.Contracts.V1.Requests;
using Models.Contracts.V1.Requests.Queries;
using Models.Contracts.V1.Responses;
using Models.Domain;
using Models.Interfaces;

namespace blog_rest_api.Controllers.V1
{
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class BlogsController : Controller
    {
        private readonly IBlogService _blogService;
        private readonly IMapper _mapper;
        private readonly IUriService _uriService;
        public BlogsController(IBlogService blogService, IMapper mapper, IUriService uriService)
        {
            _blogService = blogService;
            _mapper = mapper;
            _uriService = uriService;
        }

        [HttpGet(ApiRoutes.Blogs.GetAll)]
        public async Task<IActionResult> GetAll([FromQuery] string? userId = null, [FromQuery] PaginationQuery? paginationQuery = null)
        {
            var paginationFilter = _mapper.Map<PaginationFilter>(paginationQuery);
            var blogs = await _blogService.GetAllAsync(userId, paginationFilter);
            var blogsResponse = _mapper.Map<List<BlogResponse>>(blogs);

            if (paginationFilter == null || paginationFilter.PageNumber < 1 || paginationFilter.PageSize < 1)
            {
                return Ok(new PagedResponse<BlogResponse>(blogsResponse));
            }

            var paginationResponse = PaginationHelpers.CreatePaginatedResponse(_uriService, paginationFilter, blogsResponse);
            return Ok(paginationResponse);
        }

        [HttpGet(ApiRoutes.Blogs.Get)]
        public async Task<IActionResult> Get([FromRoute] string blogId)
        {
            var blog = await _blogService.GetByIdAsync(blogId);

            if (blog == null)
                return NotFound();

            return Ok(new Response<BlogResponse>(_mapper.Map<BlogResponse>(blog)));
        }

        [HttpPost(ApiRoutes.Blogs.Create)]
        public async Task<IActionResult> Create([FromBody] CreateBlogRequest blogRequest)
        {
            var newBlogId = Guid.NewGuid().ToString();
            var blog = new Blog
            {
                Id = newBlogId,
                Name = blogRequest.Name,
                UserId = HttpContext.GetUserId(),
                Tags = blogRequest.Tags.Select(x => new BlogTag { BlogId = newBlogId, TagId = x.ToLower() }).ToList(),
                CreatedAt = DateTime.Now.ToLocalTime(),
                UpdatedAt = DateTime.Now.ToLocalTime()
            };

            var result = await _blogService.CreateBlogAsync(blog);

            if (!result)
                return BadRequest();

            var location = _uriService.GetBlogUri(blog.Id.ToString());

            return Created(location, new Response<BlogResponse>(_mapper.Map<BlogResponse>(blog)));
        }

        [HttpPut(ApiRoutes.Blogs.Update)]
        public async Task<IActionResult> Update([FromRoute] string blogId, [FromBody] UpdateBlogRequest request)
        {
            var userOwnPost = await _blogService.UserOwnsBlogAsync(blogId.ToString(), HttpContext.GetUserId());

            if (!userOwnPost)
            {
                return BadRequest(new { error = "You do nor own this blog" });

            }

            var blog = await _blogService.GetByIdAsync(blogId);
            blog.Name = request.Name;

            var updated = await _blogService.UpdateBlogAsync(blog);

            if (updated)
                return Ok(new Response<BlogResponse>(_mapper.Map<BlogResponse>(blog)));

            return NotFound();
        }

        [HttpDelete(ApiRoutes.Blogs.Delete)]
        public async Task<IActionResult> Delete([FromRoute] string blogId)
        {
            var userOwnPost = await _blogService.UserOwnsBlogAsync(blogId.ToString(), HttpContext.GetUserId());

            if (!userOwnPost)
            {
                return BadRequest(new { error = "You do nor own this blog" });

            }

            var deleted = await _blogService.DeleteBlogAsync(blogId);

            if (!deleted)
                return NotFound();

            return NoContent();
        }
    }
}
