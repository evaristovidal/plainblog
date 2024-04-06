using PlainBlog.Post.Api.Extensions;
using PlainBlog.i18n.Extensions;
using PlainBlog.Store.Extensions;
using PlainBlog.Author.Extensions;

namespace PlainBlog.Application.Extensions;

public static class ApplicationCoreExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services, string connectionString)
    {
        services.AddDbContext(connectionString);
        services.AddTranslationProviders();
        services.AddAuthorCore();
        services.AddPostProviders();
        return services;
    }
}
