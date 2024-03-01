using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using ParsaLibraryManagement.Domain.Entities;

namespace ParsaLibraryManagement.Infrastructure.Data.Configurations
{
    /// <summary>
    ///     Configuration for the entity type <see cref="Gender"/> in the database context.
    /// </summary>
    /// <remarks>
    ///     This class defines how the Gender entity should be mapped to the database, including its properties and relationships.
    /// </remarks>
    public class GenderConfiguration : IEntityTypeConfiguration<Gender>
    {
        public void Configure(EntityTypeBuilder<Gender> builder)
        {
            builder.HasKey(e => e.GenderId);
            builder.Property(e => e.GenderId)
                .ValueGeneratedOnAdd();

            builder.Property(e => e.Code)
                .IsRequired()
                .HasMaxLength(10)
                .IsUnicode(false);

            builder.Property(e => e.Title)
                .IsRequired()
                .HasMaxLength(20)
                .IsUnicode(true);

            // Ensure Code is unique
            builder.HasIndex(e => e.Code, "IX_Genders_Code").IsUnique();

            // Seed data
            builder.HasData(
                new Gender { GenderId = 1, Code = "M", Title = "Male" },
                new Gender { GenderId = 2, Code = "F", Title = "Female" },
                new Gender { GenderId = 3, Code = "RNS", Title = "Rather Not Say" },
                new Gender { GenderId = 4, Code = "MXD", Title = "Mixed" });

            // Navigation properties

        }
    }
}
