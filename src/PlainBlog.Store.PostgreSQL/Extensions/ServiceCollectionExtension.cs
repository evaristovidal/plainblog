using Microsoft.Extensions.DependencyInjection;
using PlainBlog.Store.Extensions;

namespace PlainBlog.Store.PostgreSQL.Extensions;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddArDbContext(this IServiceCollection services)
    {
        services.AddDbContextResolver();
        services.AddDbContext<AbstractPlainBlogContext>(ServiceLifetime.Scoped);

        return services;
    }
}
