using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models.Contracts.V1;
using Models.Contracts.V1.Requests;

namespace blog_rest_api.Controllers.V1
{
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class CommentController : Controller
    {
        public CommentController() { }

        [HttpGet(ApiRoutes.Comments.GetAll)]
        public async Task<IActionResult> GetAll()
        {
            return Ok();
        }

        [HttpGet(ApiRoutes.Comments.Get)]
        public async Task<IActionResult> Get(string commentId)
        {
            return Ok();
        }

        [HttpPost(ApiRoutes.Comments.Create)]
        public async Task<IActionResult> Create([FromBody] CreateCommentRequest createCommentRequest)
        {
            return Ok();
        }

        [HttpPut(ApiRoutes.Comments.Update)]
        public async Task<IActionResult> Update([FromBody] UpdateCommentRequest updateCommentRequest)
        {
            return Ok();
        }

        [HttpDelete(ApiRoutes.Comments.Delete)]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Delete(string commentId)
        {
            return NoContent();
        }

    }
}
