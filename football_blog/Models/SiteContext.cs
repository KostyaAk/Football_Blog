using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace football_blog.Models
{
    public class SiteContext : IdentityDbContext<User>
    {    
            public SiteContext(DbContextOptions<SiteContext> options)
                : base(options)
            {

            }
            public DbSet<User> Users { get; set; }
            public DbSet<Club> Clubs { get; set; }
            public DbSet<Tag> Tags { get; set; }
            public DbSet<Post> Posts { get; set; }
            public DbSet<Comment> Comments { get; set; }
    }
    
}
