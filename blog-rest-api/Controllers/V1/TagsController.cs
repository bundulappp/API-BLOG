﻿using AutoMapper;
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
            var tags = await _blogService.GetAllTagsAsync();
            return Ok(new PagedResponse<TagResponse>(_mapper.Map<List<TagResponse>>(tags)));
        }

        [HttpPost(ApiRoutes.Tags.Create)]
        public async Task<IActionResult> Create([FromBody] CreateTagRequest tagRequest)
        {
            var newTag = _mapper.Map<Tag>(tagRequest);
            newTag.UserId = HttpContext.GetUserId();

            var result = await _blogService.CreateTagAsync(newTag);

            if (!result)
                return BadRequest();

            var baseUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host.ToUriComponent()}";
            var locationUri = baseUrl + "/" + ApiRoutes.Tags.Create;
            var response = new Response<TagResponse>(_mapper.Map<TagResponse>(newTag));

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
