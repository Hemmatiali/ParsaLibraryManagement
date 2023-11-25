using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using ParsaLibraryManagement.Domain.Entities;

namespace ParsaLibraryManagement.Infrastructure.Data.Configurations
{
    /// <summary>
    /// Configuration for the entity type <see cref="BorrowedBook"/> in the database context.
    /// </summary>
    /// <remarks>
    /// This class defines how the BorrowedBook entity should be mapped to the database, including its properties and relationships.
    /// </remarks>
    public class BorrowedBookConfiguration : IEntityTypeConfiguration<BorrowedBook>
    {
        public void Configure(EntityTypeBuilder<BorrowedBook> builder)
        {
            builder.HasKey(e => e.Bid);
            builder.Property(e => e.Bid).HasColumnName("BID");
            builder.Property(e => e.Bid).ValueGeneratedOnAdd();

            builder.HasOne(d => d.User)
                .WithMany(p => p.BorrowedBooks)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_BorrowedBooks_Users");

            builder.HasOne(d => d.Books)
                .WithMany(p => p.BorrowedBooks)
                .HasForeignKey(d => d.BookId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_BorrowedBooks_Books");

            builder.Property(e => e.IsBorrowed).HasDefaultValueSql("((0))");
        }
    }
}