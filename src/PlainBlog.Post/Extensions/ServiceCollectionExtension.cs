using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using PlainBlog.Post.Abstractions;
using PlainBlog.Post.Repository.Extensions;
using PlainBlog.Post.Validator;

namespace PlainBlog.Post.Extensions;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddPostCore(this IServiceCollection services)
    {
        services.AddPostRepositories();
        services.AddScoped<IPostManagementService, PostManagementService>();
        services.AddScoped<IValidator<PostSave>, PostSaveValidator>();

        return services;
    }
}
