using Microsoft.EntityFrameworkCore;
using PlainBlog.Post.Abstractions;
using PlainBlog.Store;

namespace PlainBlog.Post.Repository;

public class PostRepository : IPostRepository
{
    private readonly PlainBlogContext _context;

    public PostRepository(PlainBlogContext context)
    {
        _context = context;
    }

    public async Task<int> CreateAsync(PostSave model, CancellationToken token)
    {
        var newPost = new Store.Entities.Post()
        {
            AuthorId = model.AuthorId,
            Content = model.Content,
            Description = model.Description,
            Title = model.Title
        };

        await _context.Posts.AddAsync(newPost, token);
        await _context.SaveChangesAsync(token);

        return newPost.Id;

    }

    public async Task<IEnumerable<Abstractions.Post>> GetAsync(CancellationToken token)
    {
        var q = await _context.Posts
                        .Select(p => new Post.Abstractions.Post()
                        { 
                            Id = p.Id,
                            AuthorId = p.AuthorId,
                            Content = p.Content,
                            Description = p.Description,
                            Title = p.Title
                        })
                        .ToListAsync(token);

        return q;
    }

    public async Task<Abstractions.Post?> GetAsync(int postId, CancellationToken token)
    {
        var post = await _context.Posts
                        .Where(p => p.Id == postId)
                        .Select(p => new Post.Abstractions.Post()
                        {
                            Id = p.Id,
                            AuthorId = p.AuthorId,
                            Content = p.Content,
                            Description = p.Description,
                            Title = p.Title
                        })
                        .SingleOrDefaultAsync(token);

        return post;
    }
}
