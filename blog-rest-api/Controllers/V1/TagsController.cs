using AutoMapper;
using blog_rest_api.Contracts.V1;
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
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "blogger, admin")]
    public class TagsController : Controller
    {
        private readonly IBlogService _blogService;
        private readonly IMapper _mapper;
        public TagsController(IBlogService blogService, IMapper mapper)
        {
            _blogService = blogService;
            _mapper = mapper;
        }

        [HttpGet(ApiRoutes.Tags.GetAll)]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _blogService.GetAllTagsAsync());
        }

        [HttpPost(ApiRoutes.Tags.Create)]
        public async Task<IActionResult> Create([FromBody] CreateTagRequest tagRequest)
        {
            var newTag = new Tag
            {
                Name = tagRequest.Name,
                UserId = HttpContext.GetUserId(),
                CreatedAt = DateTime.Now.ToLocalTime(),
                UpdatedAt = DateTime.Now.ToLocalTime()
            };

            var result = await _blogService.CreateTagAsync(newTag);

            if (!result)
                return BadRequest();

            var baseUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host.ToUriComponent()}";
            var locationUri = baseUrl + "/" + ApiRoutes.Tags.Create;
            var response = _mapper.Map<CreateTagResponse>(newTag);

            return Created(locationUri, response);
        }

        [HttpDelete(ApiRoutes.Tags.Delete)]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Delete([FromRoute] string tagId)
        {
            var deleted = await _blogService.DeleteTagAsync(tagId);

            if (!deleted)
                return NotFound();

            return NoContent();
        }
    }
}
