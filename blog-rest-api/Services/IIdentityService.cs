using blog_rest_api.Contracts.V1.Requests;
using blog_rest_api.Domain;

namespace blog_rest_api.Services
{
    public interface IIdentityService
    {
        Task<AuthenticationResult> RegisterAsnyc(UserRegistrationRequest request);
        Task<AuthenticationResult> LoginAsync(UserLoginRequest request);

    }
}
