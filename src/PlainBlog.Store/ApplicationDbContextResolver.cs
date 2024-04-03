using Microsoft.Extensions.Configuration;

namespace PlainBlog.Store;

public class ApplicationDbContextResolver : IApplicationDbContextResolver
{
    private readonly IConfiguration _configuration;

    public ApplicationDbContextResolver(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public string GetConnectionString()
    {
        var connectionString = _configuration.GetConnectionString("PlainBlogContext");
        if (string.IsNullOrEmpty(connectionString))
            throw new Exception("ConnectionString not found");

        return connectionString;
    }
}
