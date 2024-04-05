namespace PlainBlog.Post.Abstractions;

public class Post
{
    public int Id { get; set; }
    public int AuthorId { get; set; }
    public required string Title { get; set; }
    public required string Description { get; set; }
    public required string Content { get; set; }
    public Author.Abstractions.Author? Author { get; set; }
}
