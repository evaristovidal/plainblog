using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PlainBlog.Store.Entities;

namespace PlainBlog.Store.EntityTypeConfiguration;

public class AuthorConfiguration : IEntityTypeConfiguration<Author>
{
    public void Configure(EntityTypeBuilder<Author> modelBuilder)
    {
          modelBuilder.Property(a => a.Id)
          .HasColumnName("id");

        modelBuilder
            .Property(a => a.Name)
            .HasColumnName("name")
            .HasMaxLength(250)
            .IsRequired();

        modelBuilder
            .Property(a => a.Surname)
            .HasColumnName("surname")
            .HasMaxLength(250);
    }
}
