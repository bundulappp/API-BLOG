using Models.Contracts.V1.Requests;
using Models.Domain;

namespace Models.Interfaces
{
    public interface IIdentityService
    {
        Task<AuthenticationResult> RegisterAsnyc(UserRegistrationRequest request);
        Task<AuthenticationResult> LoginAsync(UserLoginRequest request);
        Task<AuthenticationResult> RefreshTokenAsync(string token, string refreshToken);
    }
}
