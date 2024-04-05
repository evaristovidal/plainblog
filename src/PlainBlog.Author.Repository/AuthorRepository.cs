using Microsoft.EntityFrameworkCore;
using PlainBlog.Store;

namespace PlainBlog.Author.Repository;

public class AuthorRepository : IAuthorRepository
{
    private readonly AbstractPlainBlogContext _context;

    public AuthorRepository(AbstractPlainBlogContext context)
    {
        _context = context;
    }

    public async Task<Abstractions.Author?> GetAsync(int authorId, CancellationToken token)
    {
        var author = await _context.Authors
            .Where(a => authorId == a.Id)
            .Select(a => new Author.Abstractions.Author()
            {
                Id = a.Id,
                Name = a.Name,
                Surname = a.Surname,
            })
            .FirstOrDefaultAsync(token);

        return author;
    }
}
