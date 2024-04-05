namespace PlainBlog.Author.Abstractions;

public interface IAuthorManagementService
{
    Task<Abstractions.Author?> GetAsync(int authorId, CancellationToken token);
}
