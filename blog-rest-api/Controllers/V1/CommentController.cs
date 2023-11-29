using AutoMapper;
using blog_rest_api.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models.Contracts.V1;
using Models.Contracts.V1.Requests;
using Models.Contracts.V1.Responses;
using Models.Domain;
using Models.Interfaces;

namespace blog_rest_api.Controllers.V1
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class CommentController : Controller
    {
        private readonly IMapper _mapper;
        private readonly IUriService _uriService;
        private readonly ICommentService _commentService;
        public CommentController(IMapper mapper, IUriService uriService, ICommentService commentService)
        {
            _mapper = mapper;
            _uriService = uriService;
            _commentService = commentService;
        }

        [HttpGet(ApiRoutes.Comments.Get)]
        public async Task<IActionResult> Get(string commentId)
        {
            return Ok();
        }

        [HttpPost(ApiRoutes.Comments.Create)]
        public async Task<IActionResult> Create([FromBody] CreateCommentRequest createCommentRequest)
        {
            var comment = _mapper.Map<Comment>(createCommentRequest);
            comment.UserId = HttpContext.GetUserId();

            var result = await _commentService.CreateCommentAsnyc(comment);

            if (!result)
                return BadRequest();

            var location = _uriService.GetBlogUri(comment.Id.ToString());

            return Created(location, new Response<CommentResponse>(_mapper.Map<CommentResponse>(comment)));
        }

        [HttpPut(ApiRoutes.Comments.Update)]
        public async Task<IActionResult> Update([FromRoute] string commentId, [FromBody] UpdateCommentRequest updateCommentRequest)
        {
            var userOwnsComment = await _commentService.UserOwnsComment(commentId, HttpContext.GetUserId());

            if (!userOwnsComment)
                return BadRequest(new { error = "You do not own this comment!" });

            var comment = await _commentService.Get(commentId);
            comment.Body = updateCommentRequest.Body;

            var updated = await _commentService.UpdateCommentAsync(comment);

            if (updated)
                return Ok(new Response<CommentResponse>(_mapper.Map<CommentResponse>(comment)));

            return NotFound();
        }

        [HttpDelete(ApiRoutes.Comments.Delete)]
        public async Task<IActionResult> Delete(string commentId)
        {
            var userOwnsComment = await _commentService.UserOwnsComment(commentId, HttpContext.GetUserId());

            if (!userOwnsComment)
                return BadRequest(new { error = "You do not own this comment!" });

            var deleted = await _commentService.DeleteCommentAsync(commentId);

            if (!deleted)
                return NotFound();

            return NoContent();
        }

    }
}
