using Microsoft.Extensions.DependencyInjection;

namespace PlainBlog.Store.Extensions;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddDbContextResolver(this IServiceCollection services)
    {
        services.AddScoped<IApplicationDbContextResolver, ApplicationDbContextResolver>();

        return services;
    }
}