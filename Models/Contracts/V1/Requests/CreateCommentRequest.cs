namespace Models.Contracts.V1.Requests
{
    public class CreateCommentRequest
    {
        public string Body { get; set; }
        public string BlogId { get; set; }
    }
}
