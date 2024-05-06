using Microsoft.EntityFrameworkCore;
using ParsaLibraryManagement.Domain.Entities;
using ParsaLibraryManagement.Infrastructure.Data.Configurations;

namespace ParsaLibraryManagement.Infrastructure.Data.Contexts;

/// <summary>
///     Represents the database context for the Parsa Library Management system.
/// </summary>
/// <remarks>
///     This DbContext is responsible for interacting with the underlying database to manage library-related data.
/// </remarks>
public class ParsaLibraryManagementDbContext : DbContext
{
    #region Ctor

    public ParsaLibraryManagementDbContext()
    { }

    public ParsaLibraryManagementDbContext(DbContextOptions<ParsaLibraryManagementDbContext> options) : base(options)
    { }

    #endregion

    #region DbSets

    public virtual DbSet<Book> Books { get; set; }
    public virtual DbSet<BookCategory> BooksCategories { get; set; }
    public virtual DbSet<Publisher> Publishers { get; set; }
    public virtual DbSet<BorrowedBook> BorrowedBooks { get; set; }
    public virtual DbSet<Gender> Genders { get; set; }
    public virtual DbSet<User> Users { get; set; }

    #endregion

    #region Methods

    /// <summary>
    ///     Overrides the default behavior of building the model for the ParsaLibraryManagementDbContext.
    /// </summary>
    /// <param name="modelBuilder">The ModelBuilder instance used to construct the database model.</param>
    /// <remarks>
    ///     This method allows customization of the database model, including configuring entity relationships, constraints, etc.
    /// </remarks>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new BookConfiguration());
        modelBuilder.ApplyConfiguration(new BookCategoryConfiguration());
        modelBuilder.ApplyConfiguration(new PublisherConfiguration());
        modelBuilder.ApplyConfiguration(new BorrowedBookConfiguration());
        modelBuilder.ApplyConfiguration(new GenderConfiguration());
        modelBuilder.ApplyConfiguration(new UserConfiguration());
    }

    #endregion
}