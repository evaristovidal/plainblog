using Microsoft.Extensions.DependencyInjection;
using PlainBlog.Store.Extensions;

namespace PlainBlog.Store.PostgreSQL.Extensions;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddDbContext(this IServiceCollection services)
    {
        services.AddDbContextResolver();
        services.AddDbContext<AbstractPlainBlogContext, PlainBlogContext>(ServiceLifetime.Scoped);

        return services;
    }
}
