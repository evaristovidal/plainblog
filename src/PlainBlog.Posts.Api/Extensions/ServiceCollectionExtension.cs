using Microsoft.Extensions.DependencyInjection;

namespace PlainBlog.Posts.Api.Extensions;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddPostProviders(this IServiceCollection services)
    {
        //services.AddPeriodCore();

        return services;
    }
}
