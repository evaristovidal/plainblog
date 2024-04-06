using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PlainBlog.Store;
using PlainBlog.Store.PostgreSQL;
using System.Data.Common;
using System.Xml.Linq;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace PlainBlog.Post.IntegrationTests;


public class CustomWebApplicationFactory<TProgram>
    : WebApplicationFactory<TProgram> where TProgram : class
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            // Remove already loaded services
            var types = new List<Type>()
            {
                typeof(DbContextOptions<Store.PostgreSQL.PlainBlogContext>),
                typeof(DbContextOptions<Store.AbstractPlainBlogContext>),
                typeof(DbConnection),
                typeof(ApplicationDbContextResolver),
                typeof(IApplicationDbContextResolver)
            };
            foreach(var t in types)
            {
                var dbContextDescriptor1 = services.SingleOrDefault(d => d.ServiceType == t);
                if (dbContextDescriptor1 != null)
                {
                    services.Remove(dbContextDescriptor1);
                }
            }

            // Create open SqliteConnection so EF won't automatically close it.
            services.AddSingleton<IApplicationDbContextResolver>(container =>
            {
                var connection = new ApplicationDbContextResolverIntegrationtTest();

                return connection;
            });

            services.AddSingleton<DbConnection>(container =>
            {
                var connection = new SqliteConnection("DataSource=:memory:");
                connection.Open();

                return connection;
            });

            services.AddDbContext<AbstractPlainBlogContext, PlainBlogContext>((container, options) =>
            {
                var connection = container.GetRequiredService<DbConnection>();
                var resolver = container.GetRequiredService<IApplicationDbContextResolver>();
                options.UseSqlite(connection);

                // Create the schema and seed some data
                var _options = new DbContextOptionsBuilder<PlainBlogContext>()
                    .UseSqlite(connection)
                    .Options;
                using var context = new PlainBlogContext(_options, resolver);
                context.Database.Migrate();
            });
        });

        builder.UseEnvironment("IntegrationTests");
    }
}

public class ApplicationDbContextResolverIntegrationtTest : IApplicationDbContextResolver
{
    public string GetConnectionString()
    {
        var connectionString = "DataSource=:memory:";

        return connectionString;
    }
}