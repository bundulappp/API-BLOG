using blog_rest_api.Data.Domain;
using Models.Contracts.V1.Requests;

namespace blog_rest_api.Services
{
    public interface IIdentityService
    {
        Task<AuthenticationResult> RegisterAsnyc(UserRegistrationRequest request);
        Task<AuthenticationResult> LoginAsync(UserLoginRequest request);
        Task<AuthenticationResult> RefreshTokenAsync(string token, string refreshToken);
    }
}
