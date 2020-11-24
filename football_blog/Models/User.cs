using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace football_blog.Models
{
    public class User : IdentityUser
    {
        public bool isBlocked { get; set; }
        public ICollection<Comment> Comments { get; set; }
        public ICollection<Post> Posts { get; set; }
    }
}
