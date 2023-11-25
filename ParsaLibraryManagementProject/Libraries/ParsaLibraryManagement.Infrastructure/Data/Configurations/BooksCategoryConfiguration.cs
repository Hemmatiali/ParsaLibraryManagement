using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using ParsaLibraryManagement.Domain.Entities;

namespace ParsaLibraryManagement.Infrastructure.Data.Configurations
{
    /// <summary>
    /// Configuration for the entity type <see cref="BooksCategory"/> in the database context.
    /// </summary>
    /// <remarks>
    /// This class defines how the BooksCategory entity should be mapped to the database, including its properties and relationships.
    /// </remarks>
    public class BooksCategoryConfiguration : IEntityTypeConfiguration<BooksCategory>
    {
        public void Configure(EntityTypeBuilder<BooksCategory> builder)
        {
            builder.HasKey(e => e.CategoryId);
            builder.Property(e => e.CategoryId).ValueGeneratedOnAdd();

            builder.Property(e => e.Title)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(e => e.ImageAddress)
                .IsRequired()
                .HasMaxLength(37)
                .IsUnicode(false);

            builder.Property(e => e.RefId).HasComment("If it is null then it will be a SuperGroup, but if it is not Null then it will be a SubGroup.");
        }
    }
}
