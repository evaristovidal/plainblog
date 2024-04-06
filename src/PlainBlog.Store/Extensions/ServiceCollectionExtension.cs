using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace PlainBlog.Store.Extensions;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddDbContext(this IServiceCollection services, string connectionString)
    {
        services.AddDbContext<PlainBlogContext>(options =>
        {
            options.UseNpgsql(connectionString, options => options.UseQuerySplittingBehavior(QuerySplittingBehavior.SingleQuery));
        },
        ServiceLifetime.Scoped);

        return services;
    }
}