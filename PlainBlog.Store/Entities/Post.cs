using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PlainBlog.Store.Entities;

public class Post
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public int AutorId { get; set; }
    public required string Title { get; set; }
    public required string Description { get; set; }
    public required string Content { get; set; }

    public Author? Author { get; set; }
}
