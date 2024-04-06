using Microsoft.EntityFrameworkCore;

namespace PlainBlog.Store.PostgreSQL;

public class PlainBlogContext(
    DbContextOptions<PlainBlogContext> options,
    IApplicationDbContextResolver _resolver
    ) : AbstractPlainBlogContext(options)
{
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if(!optionsBuilder.IsConfigured)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseNpgsql(_resolver.GetConnectionString(), options => options.UseQuerySplittingBehavior(QuerySplittingBehavior.SingleQuery));
        }
    }
}
