using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using ParsaLibraryManagement.Domain.Entities;

namespace ParsaLibraryManagement.Infrastructure.Data.Configurations
{
    /// <summary>
    /// Configuration for the entity type <see cref="Publisher"/> in the database context.
    /// </summary>
    /// <remarks>
    /// This class defines how the Publisher entity should be mapped to the database, including its properties and relationships.
    /// </remarks>
    public class PublisherConfiguration : IEntityTypeConfiguration<Publisher>
    {
        public void Configure(EntityTypeBuilder<Publisher> builder)
        {
            builder.HasKey(e => e.PublisherId);
            builder.Property(e => e.PublisherId).ValueGeneratedOnAdd();

            builder.Property(e => e.FirstName)
                .IsRequired()
                .HasMaxLength(30);

            builder.Property(e => e.LastName)
                .IsRequired()
                .HasMaxLength(30);

            builder.HasOne(d => d.Gender)
                .WithMany(p => p.Publishers)
                .HasForeignKey(d => d.GenderId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Publishers_Genders");

            builder.Property(e => e.Email)
                .IsRequired()
                .HasMaxLength(50)
                .IsUnicode(false);

            builder.Property(e => e.PhoneNumber)
                .IsRequired()
                .HasMaxLength(11)
                .IsUnicode(false)
                .HasComment("This is only for Iranian phone numbers - This field is null for foreign users.");
        }
    }
}
