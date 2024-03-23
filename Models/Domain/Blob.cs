using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models.Domain
{
    public class Blob
    {
        [Key]
        public string Id { get; set; }
        public string BlogId { get; set; }
        [ForeignKey(nameof(BlogId))]
        public Blog Blog { get; set; }
        public string UserId { get; set; }
        [ForeignKey(nameof(UserId))]
        public IdentityUser User { get; set; }
        public BlobType BlobType { get; set; }
        public string Url { get; set; }
        public float Size { get; set; }
    }

    public enum BlobType
    {
        Image,
        Video
    }
}
