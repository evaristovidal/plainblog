using PlainBlog.Posts.Api.Extensions;

namespace PlainBlog.Application.Extensions;

public static class ApplicationCoreExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddPostProviders();
        return services;
    }
}
