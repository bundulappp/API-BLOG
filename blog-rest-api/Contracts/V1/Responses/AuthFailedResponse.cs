namespace blog_rest_api.Contracts.V1.Responses
{
    public class AuthFailedResponse
    {
        public IEnumerable<string> Errors { get; set; }
    }
}
