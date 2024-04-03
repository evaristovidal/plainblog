using Microsoft.Extensions.DependencyInjection;
using PlainBlog.Post.Extensions;

namespace PlainBlog.Post.Api.Extensions;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddPostProviders(this IServiceCollection services)
    {
        services.AddPostCore();

        return services;
    }
}
