using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PlainBlog.Store.Entities;

public class Author
{
    public Author()
    {
        Posts = new HashSet<Post>();
    }

    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public required string Name { get; set; }
    public string? Surname { get; set; }

    public ICollection<Post> Posts { get; set; }
}
