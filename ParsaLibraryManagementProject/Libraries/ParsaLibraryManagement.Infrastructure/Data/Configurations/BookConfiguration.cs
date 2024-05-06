using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ParsaLibraryManagement.Domain.Entities;

namespace ParsaLibraryManagement.Infrastructure.Data.Configurations;

/// <summary>
///     Configuration for the entity type <see cref="Book"/> in the database context.
/// </summary>
/// <remarks>
///     This class defines how the Books entity should be mapped to the database, including its properties and relationships.
/// </remarks>
public class BookConfiguration : IEntityTypeConfiguration<Book>
{
    public void Configure(EntityTypeBuilder<Book> builder)
    {
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).ValueGeneratedOnAdd();

        builder.Property(e => e.Name)
            .IsRequired()
            .HasMaxLength(50);

        builder.Property(e => e.ImageAddress)
            .IsRequired()
            .HasMaxLength(37)
            .IsUnicode(false);

        builder.Property(e => e.PageCount).HasDefaultValueSql("((1))");

        builder.Property(e => e.Price).HasColumnType("decimal(10, 0)");

        builder.Property(e => e.IsAvailable)
            .IsRequired()
            .HasDefaultValueSql("((0))")
            .HasComment("-0 NotAvailable -1 Available");

        // Navigation properties

        builder.HasOne(d => d.Category)
            .WithMany(p => p.Books)
            .HasForeignKey(d => d.CategoryId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK_Books_BooksCategories");

        builder.HasOne(d => d.Publisher)
            .WithMany(p => p.Books)
            .HasForeignKey(d => d.PublisherId)
            .OnDelete(DeleteBehavior.ClientSetNull)
            .HasConstraintName("FK_Books_Publishers");
    }
}