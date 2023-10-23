namespace blog_rest_api.Contracts.V1.Requests
{
    public class CreateTagRequest
    {
        public string Name { get; set; }
        public string UserId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
