using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models.Domain
{
    public class BlogTag
    {
        [Key]
        public Guid Id { get; set; }
        public Guid BlogId { get; set; }
        [ForeignKey(nameof(BlogId))]
        public virtual Blog Blog { get; set; }
        public string TagId { get; set; }
        [ForeignKey(nameof(TagId))]
        public virtual Tag Tag { get; set; }
    }
}
