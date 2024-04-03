using FluentValidation;
using Microsoft.Extensions.Logging;
using PlainBlog.Post.Abstractions;
using PlainBlog.Post.Repository;

namespace PlainBlog.Post;

public class PostManagementService : IPostManagementService
{
    private readonly IPostRepository _postRepository;
    private readonly IValidator<PostSave> _validator;
    private readonly ILogger _logger;

    public PostManagementService(IPostRepository postRepository, IValidator<PostSave> validator, ILogger<PostManagementService> logger)
    {
        _postRepository = postRepository;
        _validator = validator;
        _logger = logger;
    }

    public async Task<int> CreateAsync(PostSave model, CancellationToken token)
    {
        ArgumentNullException.ThrowIfNull(model);
        await ValidateModelAsync(model, token);

        _logger.LogDebug("Post model has been validated. Creating a new Post");
        var postId = await _postRepository.CreateAsync(model, token);

        return postId;
    }

    public Task<IEnumerable<Abstractions.Post>> GetAsync(CancellationToken token)
    {
        return _postRepository.GetAsync(token);
    }

    public Task<Abstractions.Post?> GetAsync(int postId, CancellationToken token)
    {
        return _postRepository.GetAsync(postId, token);
    }

    private async Task ValidateModelAsync(PostSave model, CancellationToken token)
    {
        var result = await _validator.ValidateAsync(model, token);

        if (!result.IsValid)
        {
            throw new FluentValidation.ValidationException(result.Errors);
        }
    }
}
