using PlainBlog.Post.Api.Extensions;
using PlainBlog.i18n.Extensions;
using PlainBlog.Store.PostgreSQL.Extensions;

namespace PlainBlog.Application.Extensions;

public static class ApplicationCoreExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddDbContext();
        services.AddTranslationProviders();
        services.AddPostProviders();
        return services;
    }
}
