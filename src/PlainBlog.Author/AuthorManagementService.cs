using Microsoft.Extensions.Logging;
using PlainBlog.Author.Abstractions;
using PlainBlog.Author.Repository;

namespace PlainBlog.Author;

public class AuthorManagementService : IAuthorManagementService
{
    private readonly IAuthorRepository _postRepository;

    public AuthorManagementService(IAuthorRepository postRepository, ILogger<AuthorManagementService> logger)
    {
        _postRepository = postRepository;
    }

    public Task<Abstractions.Author?> GetAsync(int authorId, CancellationToken token)
    {
        return _postRepository.GetAsync(authorId, token);
    }
}
