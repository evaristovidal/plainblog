using Microsoft.Extensions.DependencyInjection;

namespace PlainBlog.Author.Repository.Extensions;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddAuthorRepositories(this IServiceCollection services)
    {
        services.AddScoped<IAuthorRepository, AuthorRepository>();

        return services;
    }
}
