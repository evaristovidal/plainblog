using Microsoft.Extensions.DependencyInjection;
using PlainBlog.Author.Abstractions;
using PlainBlog.Author.Repository.Extensions;

namespace PlainBlog.Author.Extensions;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddAuthorCore(this IServiceCollection services)
    {
        services.AddAuthorRepositories();
        services.AddScoped<IAuthorManagementService, AuthorManagementService>();

        return services;
    }
}
