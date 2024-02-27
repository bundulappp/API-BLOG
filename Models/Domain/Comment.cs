using Microsoft.AspNetCore.Identity;
using Models.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models.Domain
{
    public class Comment : IUserOwnedEntity
    {
        [Key]
        public string Id { get; set; }
        public string UserId { get; set; }
        [ForeignKey(nameof(UserId))]
        public IdentityUser User { get; set; }
        public string Body { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public int LikesCounter { get; set; }
        public string BlogId { get; set; }
        [ForeignKey(nameof(BlogId))]
        public virtual Blog Blog { get; set; }
        public string ParentCommentId { get; set; }
        [ForeignKey(nameof(ParentCommentId))]
        public virtual Comment ParentComment { get; set; }
        public virtual List<Comment> ChildComments { get; set; }
    }
}
