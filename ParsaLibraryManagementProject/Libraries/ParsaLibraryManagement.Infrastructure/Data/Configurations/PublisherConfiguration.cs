using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using ParsaLibraryManagement.Domain.Entities;

namespace ParsaLibraryManagement.Infrastructure.Data.Configurations
{
    /// <summary>
    ///     Configuration for the entity type <see cref="Publisher"/> in the database context.
    /// </summary>
    /// <remarks>
    ///     This class defines how the Publisher entity should be mapped to the database, including its properties and relationships.
    /// </remarks>
    public class PublisherConfiguration : IEntityTypeConfiguration<Publisher>
    {
        public void Configure(EntityTypeBuilder<Publisher> builder)
        {
            builder.HasKey(e => e.PublisherId);
            builder.Property(e => e.PublisherId)
                .ValueGeneratedOnAdd()
                .HasDefaultValueSql("newid()");

            builder.Property(e => e.FirstName)
                .IsRequired()
                .HasMaxLength(30);

            builder.Property(e => e.LastName)
                .IsRequired()
                .HasMaxLength(30);

            builder.Property(e => e.Email)
                .IsRequired()
                .HasMaxLength(50)
                .IsUnicode(false);

            // Ensure Email is unique
            builder.HasIndex(e => e.Email, "IX_Publishers_Email").IsUnique();

            // Navigation properties

            builder.HasOne(d => d.Gender)
                .WithMany(p => p.Publishers)
                .HasForeignKey(d => d.GenderId)
                .OnDelete(DeleteBehavior.ClientNoAction)
                .HasConstraintName("FK_Publishers_Genders");
        }
    }
}
