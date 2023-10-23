using AutoMapper;
using blog_rest_api.Contracts.V1;
using blog_rest_api.Services;
using Microsoft.AspNetCore.Mvc;

namespace blog_rest_api.Controllers.V1
{
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

        //[HttpPost(ApiRoutes.Tags.Create)]
        //public async Task<IActionResult> Create(Tag tag)
        //{
        //    var baseUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host.ToUriComponent()}";
        //    var locationUri = baseUrl + "/" + ApiRoutes.Tags.Create;
        //    var response = _mapper.Map<CreateTagResponse>(tag);

        //    return Created(locationUri, response);
        //}
    }
}
