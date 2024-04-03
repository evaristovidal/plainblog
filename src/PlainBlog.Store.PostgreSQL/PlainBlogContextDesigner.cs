using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PlainBlog.Store.PostgreSQL;

namespace PlainBlog.Store;

public class PlainBlogContextDesigner : IDesignTimeDbContextFactory<PlainBlogContext>
{
    public PlainBlogContext CreateDbContext(string[] args)
    {
        // Create an instance of the IServiceProvider (you might need to adjust this based on your actual service configuration)
        var serviceProvider = new ServiceCollection()
            .AddScoped<IApplicationDbContextResolver, ApplicationDbContextResolverDesigner>()
            .AddDbContext<AbstractPlainBlogContext, PlainBlogContext>(ServiceLifetime.Scoped)
            .BuildServiceProvider();
        var dbResolver = serviceProvider.GetRequiredService<IApplicationDbContextResolver>();

        // get connectionstring
        string connectionString = dbResolver.GetConnectionString()
            ?? throw new Exception("To use PlainBlogContextDesigner the connectionstring PlainBlogContext must be present in the appsetting.json");

        // initialize the PostgreSql connection
        var optionsBuilder = new DbContextOptionsBuilder<PlainBlogContext>();
        optionsBuilder.UseNpgsql(connectionString, options => options.UseQuerySplittingBehavior(QuerySplittingBehavior.SingleQuery));

        return new PlainBlogContext(optionsBuilder.Options, dbResolver);
    }
}

public sealed class ApplicationDbContextResolverDesigner : IApplicationDbContextResolver
{
    readonly string connectionString = "";

    public ApplicationDbContextResolverDesigner()
    {
        // Build configuration to retrieve the connectionstring from appsettings.json
        string currentDirectory = Directory.GetCurrentDirectory();
        IConfigurationRoot configuration = new ConfigurationBuilder()
            .SetBasePath(currentDirectory + @"\..\PlainBlog.Application\")
            .AddJsonFile("appsettings.json")
            .Build();
        connectionString = configuration.GetConnectionString("PlainBlogContext")
            ?? throw new Exception("To use ProphixARContextDesigner the connectionstring ProphixARContext must be present in the appsetting.json");
    }

    string IApplicationDbContextResolver.GetConnectionString()
    {
        return connectionString;
    }
}