using Microsoft.EntityFrameworkCore;

namespace SharpAgent.Infrastructure.Data;

// The "Primary Constructor" allows us to place parameters to be specified directly in the class declaration.
// Using DbContextOptions allows options to be configured at a higher level like in the program.cs
// of the Presentation layer using the appsetting.json configuration
internal class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    // The standard constructor (commented below) is replaced by the primary constructor syntax above.
    // public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) {}

    // Define your "internal" DbSet<YourEntity>s here

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Customize model creation code, such as configuring entity relationships, keys, etc.

        base.OnModelCreating(modelBuilder);
    }
}
