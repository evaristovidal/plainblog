using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlainBlog.Post.Repository.Extensions;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddPostRepositories(this IServiceCollection services)
    {
        services.AddScoped<IPostRepository, PostRepository>();

        return services;
    }
}
