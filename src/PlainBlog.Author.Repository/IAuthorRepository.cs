namespace PlainBlog.Author.Repository;

public interface IAuthorRepository
{
    Task<Abstractions.Author?> GetAsync(int authorId, CancellationToken token);
}
