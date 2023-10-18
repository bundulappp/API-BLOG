using System.ComponentModel.DataAnnotations;

namespace blog_rest_api.Contracts.V1.Requests
{
    public class UserRegistrationRequest
    {
        [EmailAddress]
        public string Email { get; set; }
        public string Password { get; set; }
        public string Username { get; set; }
    }
}
