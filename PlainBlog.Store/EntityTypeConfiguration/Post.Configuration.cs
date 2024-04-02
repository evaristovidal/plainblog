using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PlainBlog.Store.Entities;

namespace PlainBlog.Store.EntityTypeConfiguration;

public class PostConfiguration : IEntityTypeConfiguration<Post>
{
    public void Configure(EntityTypeBuilder<Post> modelBuilder)
    {
        modelBuilder
          .Property(a => a.Id)
          .HasColumnName("id");

        modelBuilder
           .HasIndex(a => new { a.Title, a });

        modelBuilder
           .Property(a => a.Title)
           .HasColumnName("author_id");

        modelBuilder
            .Property(a => a.Title)
            .HasColumnName("title")
            .HasMaxLength(100)
            .IsRequired();

        modelBuilder
            .Property(a => a.Description)
            .HasColumnName("description")
            .HasMaxLength(250)
            .IsRequired();

        modelBuilder
            .Property(a => a.Content)
            .HasColumnName("content")
            .IsRequired();
    }
}
