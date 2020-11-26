using football_blog.Models;
using System.Collections.Generic;

namespace football_blog.ViewModels
{
    public class PostIndexViewModel
    {
        public IEnumerable<Tag> Tags { get; set; }
        public IEnumerable<Post> Posts { get; set; }
    }
}
