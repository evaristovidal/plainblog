using Microsoft.EntityFrameworkCore;
using PlainBlog.Store.Entities;
using PlainBlog.Store.EntityTypeConfiguration;

namespace PlainBlog.Store;

public class AbstractPlainBlogContext : DbContext
{
    public virtual DbSet<Author> Authors { get; set; }
    public virtual DbSet<Post> Posts { get; set; }

    protected AbstractPlainBlogContext(DbContextOptions options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new AuthorConfiguration());
        modelBuilder.ApplyConfiguration(new PostConfiguration());
        
        base.OnModelCreating(modelBuilder);
    }
}
