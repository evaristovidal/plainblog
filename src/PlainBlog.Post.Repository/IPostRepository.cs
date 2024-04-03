namespace PlainBlog.Post.Repository;

public interface IPostRepository
{
    Task<IEnumerable<Abstractions.Post>> GetAsync(CancellationToken token);
    Task<Abstractions.Post?> GetAsync(int postId, CancellationToken token);
    Task<int> CreateAsync(Abstractions.PostSave model, CancellationToken token);
}
