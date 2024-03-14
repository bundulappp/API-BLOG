namespace Models.Contracts.V1.Requests
{
    public class ReplyCommentRequest
    {
        public string BlogId { get; set; }
        public string ParentCommentId { get; set; }
        public string Body { get; set; }

    }
}
