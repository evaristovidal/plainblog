using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PlainBlog.Store.Entities;
using PlainBlog.Store.EntityTypeConfiguration;

namespace PlainBlog.Store;

public class AbstractPlainBlogContext : DbContext
{
    private readonly IServiceProvider _serviceProvider;
    public virtual DbSet<Author> Authors { get; set; }
    public virtual DbSet<Post> Posts { get; set; }

    protected AbstractPlainBlogContext(DbContextOptions options, IServiceProvider serviceProvider) : base(options)
    {
        _serviceProvider = serviceProvider;
    }

    protected T GetConfigurationInstance<T>()
    {
        return ActivatorUtilities.CreateInstance<T>(_serviceProvider);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(GetConfigurationInstance<AuthorConfiguration>());
        modelBuilder.ApplyConfiguration(GetConfigurationInstance<PostConfiguration>());
        
        base.OnModelCreating(modelBuilder);
    }
}
