namespace PlainBlog.Store;

/// <remarks>Scoped service</remarks>
public interface IApplicationDbContextResolver
{
    /// <summary>Get the DbContext's ConnectionString for the current service scope</summary>
    string GetConnectionString();
}
