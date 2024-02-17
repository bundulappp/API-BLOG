namespace Models.Contracts.V1.Responses
{
    public class BlogResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string UserId { get; set; }
        public string Body { get; set; }
        public List<TagResponse> Tags { get; set; }
    }
}
