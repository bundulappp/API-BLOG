namespace Models.Contracts.V1.Requests
{
    public class CreateBlogRequest
    {
        public string Name { get; set; }
        public List<string> Tags { get; set; }
        public string Body { get; set; }
    }
}
