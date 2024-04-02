using Microsoft.EntityFrameworkCore;

namespace PlainBlog.Store.PostgreSQL;

public class PlainBlogContext(
    DbContextOptions<PlainBlogContext> options,
    IApplicationDbContextResolver _resolver,
    IServiceProvider serviceProvider
    ) : AbstractPlainBlogContext(options, serviceProvider)
{
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        base.OnConfiguring(optionsBuilder);
        optionsBuilder.UseNpgsql(_resolver.GetConnectionString(), options => options.UseQuerySplittingBehavior(QuerySplittingBehavior.SingleQuery));
    }
}
