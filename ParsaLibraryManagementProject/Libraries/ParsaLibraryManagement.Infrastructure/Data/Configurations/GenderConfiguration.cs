using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using ParsaLibraryManagement.Domain.Entities;

namespace ParsaLibraryManagement.Infrastructure.Data.Configurations
{
    /// <summary>
    /// Configuration for the entity type <see cref="Gender"/> in the database context.
    /// </summary>
    /// <remarks>
    /// This class defines how the Gender entity should be mapped to the database, including its properties and relationships.
    /// </remarks>
    public class GenderConfiguration : IEntityTypeConfiguration<Gender>
    {
        public void Configure(EntityTypeBuilder<Gender> builder)
        {
            builder.HasKey(e => e.GenderId);
            builder.Property(e => e.GenderId).ValueGeneratedOnAdd();

            builder.Property(e => e.Code)
                .IsRequired()
                .HasMaxLength(10)
                .IsUnicode(false);

            builder.Property(e => e.Title)
                .IsRequired()
                .HasMaxLength(10);
        }
    }
}
