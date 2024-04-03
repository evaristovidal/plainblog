namespace PlainBlog.Post.Abstractions;

public class PostSave
{
    public int AuthorId { get; set; }
    public required string Title { get; set; }
    public required string Description { get; set; }
    public required string Content { get; set; }
}
