using blog_rest_api.Contracts.V1;
using blog_rest_api.Contracts.V1.Requests;
using blog_rest_api.Contracts.V1.Responses;
using blog_rest_api.Data;
using blog_rest_api.Services;
using Microsoft.AspNetCore.Mvc;

namespace blog_rest_api.Controllers.V1
{
    public class IdentityController : Controller
    {
        private readonly IIdentityService _identityService;
        private readonly BlogDbContext _dbContext;

        public IdentityController(IIdentityService identityService, BlogDbContext dbContext)
        {
            _identityService = identityService;
            _dbContext = dbContext;
        }

        [HttpPost(ApiRoutes.Identity.Register)]
        public async Task<IActionResult> Register([FromBody] UserRegistrationRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new AuthFailedResponse
                {
                    Errors = ModelState.Values.SelectMany(x => x.Errors.Select(xx => xx.ErrorMessage))
                });
            }
            var authResponse = await _identityService.RegisterAsnyc(request);

            if (!authResponse.Success)
            {
                return BadRequest(new AuthFailedResponse
                {
                    Errors = authResponse.Errors,
                });
            }

            return Ok(new AuthSuccessResponse
            {
                Token = authResponse.Token,
            });
        }

        [HttpPost(ApiRoutes.Identity.Login)]
        public async Task<IActionResult> Login([FromBody] UserLoginRequest request)
        {
            var authResponse = await _identityService.LoginAsync(request);

            if (!authResponse.Success)
            {
                return BadRequest(new AuthFailedResponse
                {
                    Errors = authResponse.Errors,
                });
            }
            return Ok(new AuthSuccessResponse
            {
                Token = authResponse.Token,
            });
        }
    }
}
