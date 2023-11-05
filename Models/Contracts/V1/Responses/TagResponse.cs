namespace Models.Contracts.V1.Responses
{
    public class TagResponse
    {
        public string Name { get; set; }
        public Guid UserId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

    }
}
