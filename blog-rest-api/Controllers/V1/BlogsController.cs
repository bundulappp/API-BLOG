﻿using AutoMapper;
using blog_rest_api.Contracts.V1;
using blog_rest_api.Contracts.V1.Request;
using blog_rest_api.Contracts.V1.Requests;
using blog_rest_api.Contracts.V1.Requests.Queries;
using blog_rest_api.Contracts.V1.Responses;
using blog_rest_api.Domain;
using blog_rest_api.Extensions;
using blog_rest_api.Services;
using Microsoft.AspNetCore.Mvc;

namespace blog_rest_api.Controllers.V1
{
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class BlogsController : Controller
    {
        private readonly IBlogService _blogService;
        private readonly IMapper _mapper;
        public BlogsController(IBlogService blogService, IMapper mapper)
        {
            _blogService = blogService;
            _mapper = mapper;
        }

        [HttpGet(ApiRoutes.Blogs.GetAll)]
        public async Task<IActionResult> GetAll([FromQuery] PaginationQuery paginationQuery)
        {
            var paginationFilter = _mapper.Map<PaginationFilter>(paginationQuery);
            var blogs = await _blogService.GetAllAsync(paginationFilter);
            var blogsResponse = _mapper.Map<List<BlogResponse>>(blogs);
            var paginationResponse = new PagedResponse<BlogResponse>(blogsResponse);
            return Ok(paginationResponse);
        }

        [HttpGet(ApiRoutes.Blogs.Get)]
        public async Task<IActionResult> Get([FromRoute] Guid Id)
        {
            var blog = _blogService.GetByIdAsync(Id);

            if (blog == null)
                return NotFound();

            return Ok(new Response<BlogResponse>(_mapper.Map<BlogResponse>(blog)));
        }

        [HttpPost(ApiRoutes.Blogs.Create)]
        public async Task<IActionResult> Create([FromBody] CreateBlogRequest blogRequest)
        {
            var newBlogId = Guid.NewGuid();
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

            var baseUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host.ToUriComponent()}";
            var locationUri = baseUrl + "/" + ApiRoutes.Blogs.Get.Replace("{blogId}", blog.Id.ToString());

            return Created(locationUri, new Response<BlogResponse>(_mapper.Map<BlogResponse>(blog)));
        }

        [HttpPut(ApiRoutes.Blogs.Update)]
        public async Task<IActionResult> Update([FromRoute] Guid blogId, [FromBody] UpdateBlogRequest request)
        {
            var userOwnPost = await _blogService.UserOwnsPostAsync(blogId, HttpContext.GetUserId());

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
        public async Task<IActionResult> Delete([FromRoute] Guid blogId)
        {
            var userOwnPost = await _blogService.UserOwnsPostAsync(blogId, HttpContext.GetUserId());

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
