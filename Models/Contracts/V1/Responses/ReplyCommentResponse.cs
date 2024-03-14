namespace Models.Contracts.V1.Responses
{
    public class ReplyCommentResponse : CommentResponse
    {
        public string ParentId { get; set; }
    }
}
