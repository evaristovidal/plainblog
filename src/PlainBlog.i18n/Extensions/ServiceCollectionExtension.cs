using Microsoft.Extensions.DependencyInjection;

namespace PlainBlog.i18n.Extensions;

public static class ServiceCollectionExtension
{
    public static IServiceCollection AddTranslationProviders(this IServiceCollection services)
    {
        services.AddLocalization();

        return services;
    }
}
