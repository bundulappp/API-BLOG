namespace Models.Contracts.V1.Responses
{
    public class CommentResponse
    {
        public string UserId { get; set; }
        public string Body { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public int LikesCounter { get; set; }
        public string BlogId { get; set; }
    }
}
