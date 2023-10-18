using blog_rest_api.Contracts.V1;
using blog_rest_api.Contracts.V1.Request;
using blog_rest_api.Contracts.V1.Requests;
using blog_rest_api.Domain;
using blog_rest_api.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace blog_rest_api.Controllers.V1
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class BlogsController : Controller
    {
        private IBlogService _blogService;

        public BlogsController(IBlogService blogService)
        {
            _blogService = blogService;

        }
        [HttpGet(ApiRoutes.Blogs.GetAll)]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _blogService.GetAllAsync());
        }

        [HttpGet(ApiRoutes.Blogs.Get)]
        public async Task<IActionResult> Get([FromRoute] Guid Id)
        {
            var blog = _blogService.GetByIdAsync(Id);

            if (blog == null)
                return NotFound();

            return Ok(blog);
        }

        [HttpPost(ApiRoutes.Blogs.Create)]
        public async Task<IActionResult> Create([FromBody] CreateBlogRequest blogRequest)
        {
            var blog = new Blog { Name = blogRequest.Name };
            var response = await _blogService.CreateBlogAsync(blog);
            var baseUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host.ToUriComponent()}";
            var locationUri = baseUrl + "/" + ApiRoutes.Blogs.Get.Replace("{blogId}", blog.Id.ToString());

            return Created(locationUri, response);
        }

        [HttpPut(ApiRoutes.Blogs.Update)]
        public async Task<IActionResult> Update([FromRoute] Guid blogId, [FromBody] UpdateBlogRequest blogToUpdate)
        {
            var blog = new Blog
            {
                Id = blogId,
                Name = blogToUpdate.Name,
            };

            var updated = await _blogService.UpdateBlogAsync(blog);

            if (updated)
                return Ok();

            return NotFound();
        }

        [HttpDelete(ApiRoutes.Blogs.Delete)]
        public async Task<IActionResult> Delete([FromRoute] Guid blogId)
        {
            var deleted = await _blogService.DeleteBlogAsync(blogId);

            if (!deleted)
                return NotFound();

            return NoContent();
        }
    }
}
