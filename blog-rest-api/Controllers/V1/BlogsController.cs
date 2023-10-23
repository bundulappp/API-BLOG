﻿using AutoMapper;
using blog_rest_api.Contracts.V1;
using blog_rest_api.Contracts.V1.Request;
using blog_rest_api.Contracts.V1.Requests;
using blog_rest_api.Contracts.V1.Responses;
using blog_rest_api.Domain;
using blog_rest_api.Extensions;
using blog_rest_api.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace blog_rest_api.Controllers.V1
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
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
        public async Task<IActionResult> GetAll()
        {
            var blogs = await _blogService.GetAllAsync();
            return Ok(_mapper.Map<List<BlogResponse>>(blogs));
        }

        [HttpGet(ApiRoutes.Blogs.Get)]
        public async Task<IActionResult> Get([FromRoute] Guid Id)
        {
            var blog = _blogService.GetByIdAsync(Id);

            if (blog == null)
                return NotFound();

            return Ok(_mapper.Map<BlogResponse>(blog));
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

            await _blogService.CreateBlogAsync(blog);

            var baseUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host.ToUriComponent()}";
            var locationUri = baseUrl + "/" + ApiRoutes.Blogs.Get.Replace("{blogId}", blog.Id.ToString());

            return Created(locationUri, _mapper.Map<BlogResponse>(blog));
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
                return Ok();

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
