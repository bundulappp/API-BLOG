using blog_rest_api.Contracts.V1;
using blog_rest_api.Contracts.V1.Request;
using blog_rest_api.Contracts.V1.Responses;
using blog_rest_api.Domain;
using blog_rest_api.Services;
using Microsoft.AspNetCore.Mvc;

namespace blog_rest_api.Controllers.V1
{
    public class BlogsController : Controller
    {
        private IBlogService _blogService;

        public BlogsController(IBlogService blogService)
        {
            _blogService = blogService;

        }
        [HttpGet(ApiRoutes.Blogs.GetAll)]
        public IActionResult GetAll()
        {
            return Ok(_blogService.GetAll());
        }

        [HttpGet(ApiRoutes.Blogs.Get)]
        public IActionResult Get([FromRoute] Guid Id)
        {
            var blog = _blogService.GetById(Id);

            if (blog == null)
                return NotFound();

            return Ok(blog);
        }

        [HttpPost(ApiRoutes.Blogs.Create)]
        public IActionResult Create([FromBody] CreateBlogRequest blogRequest)
        {
            var blog = new Blog { Id = blogRequest.Id };

            if (blogRequest.Id != Guid.Empty)
                blog.Id = Guid.NewGuid();

            var baseUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host.ToUriComponent()}";
            var locationUri = baseUrl + "/" + ApiRoutes.Blogs.Get.Replace("{blogId}", blog.Id.ToString());

            var response = new CreateBlogResponse { Id = blog.Id };
            return Created(locationUri, response);
        }
    }
}
