namespace PlainBlog.Author.Abstractions;

public class Author
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public string? Surname { get; set; }
}
