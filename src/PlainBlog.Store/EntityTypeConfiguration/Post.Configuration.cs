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
           .Property(a => a.AuthorId)
           .HasColumnName("author_id");

        modelBuilder
            .Property(a => a.Title)
            .HasColumnName("title")
            .HasMaxLength(255)
            .IsRequired();

        modelBuilder
            .Property(a => a.Description)
            .HasColumnName("description")
            .HasMaxLength(255)
            .IsRequired();

        modelBuilder
            .Property(a => a.Content)
            .HasColumnName("content")
            .IsRequired();
    }
}
