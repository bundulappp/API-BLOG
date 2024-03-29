﻿using Microsoft.AspNetCore.Identity;
using Models.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models.Domain
{
    public class Blog : IUserOwnedEntity
    {
        [Key]
        public string Id { get; set; }
        public string Name { get; set; }
        public string UserId { get; set; }
        [ForeignKey(nameof(UserId))]
        public IdentityUser User { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string Body { get; set; }
        public virtual List<BlogTag> Tags { get; set; }
    }
}
