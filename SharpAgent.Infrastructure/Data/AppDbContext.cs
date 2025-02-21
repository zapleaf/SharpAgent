using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

using SharpAgent.Domain.Common;
using SharpAgent.Domain.Entities;

namespace SharpAgent.Infrastructure.Data;

// The "Primary Constructor" allows us to place parameters to be specified directly in the class declaration.
// Using DbContextOptions allows options to be configured at a higher level like in the program.cs
// of the Presentation layer using the appsetting.json configuration
public class AppDbContext(DbContextOptions<AppDbContext> options) : IdentityDbContext<User>(options)
{
    // The standard constructor (commented below) is replaced by the primary constructor syntax above.
    // public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) {}

    // DbSet properties for all entities
    internal DbSet<EmbeddingDocument> EmbeddingDocuments => Set<EmbeddingDocument>();
    internal DbSet<EmbeddingChunk> EmbeddingChunks => Set<EmbeddingChunk>();
    internal DbSet<EmbeddingModel> EmbeddingModels => Set<EmbeddingModel>();
    internal DbSet<EmbeddingTag> EmbeddingTags => Set<EmbeddingTag>();
    internal DbSet<EmbeddingCollection> EmbeddingCollections => Set<EmbeddingCollection>();

    public override int SaveChanges()
    {
        UpdateAuditFields();
        return base.SaveChanges();
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        UpdateAuditFields();
        return base.SaveChangesAsync(cancellationToken);
    }

    private void UpdateAuditFields()
    {
        var entries = ChangeTracker
            .Entries()
            .Where(e => e.Entity is BaseEntity && (
                e.State == EntityState.Added ||
                e.State == EntityState.Modified));

        foreach (var entityEntry in entries)
        {
            var entity = (BaseEntity)entityEntry.Entity;

            if (entityEntry.State == EntityState.Added)
            {
                entity.CreatedAt = DateTime.UtcNow;
                entity.LastModifiedAt = entity.CreatedAt;
            }
            else if (entityEntry.State == EntityState.Modified)
            {
                // Don't modify CreatedAt on updates
                Entry(entity).Property(x => x.CreatedAt).IsModified = false;
                entity.LastModifiedAt = DateTime.UtcNow;
            }
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Configure entities
        modelBuilder.Entity<EmbeddingDocument>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Filename).IsRequired();
            entity.Property(e => e.Location).IsRequired();
            entity.Property(e => e.Namespace).IsRequired();
            entity.Property(e => e.IndexName).IsRequired();
            entity.Property(e => e.MetadataJson).IsRequired(false);
            entity.Property(e => e.Description).IsRequired(false);

            // Configure relationships
            entity.HasOne(d => d.Model)
                  .WithMany(m => m.Documents)
                  .HasForeignKey(d => d.ModelId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasMany(d => d.Chunks)
                  .WithOne(c => c.Document)
                  .HasForeignKey(c => c.DocumentId)
                  .OnDelete(DeleteBehavior.Cascade);

            // Many-to-many relationships
            entity.HasMany(d => d.Tags)
                  .WithMany(t => t.Documents)
                  .UsingEntity(
                    "EmbeddingDocumentTags",
                    l => l.HasOne(typeof(EmbeddingTag)).WithMany().HasForeignKey("TagId"),
                    r => r.HasOne(typeof(EmbeddingDocument)).WithMany().HasForeignKey("DocumentId")
                  );

            entity.HasMany(d => d.Collections)
                  .WithMany(c => c.Documents)
                  .UsingEntity(
                    "EmbeddingDocumentCollections",
                    l => l.HasOne(typeof(EmbeddingCollection)).WithMany().HasForeignKey("CollectionId"),
                    r => r.HasOne(typeof(EmbeddingDocument)).WithMany().HasForeignKey("DocumentId")
                  );
        });

        modelBuilder.Entity<EmbeddingChunk>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.ChunkText).IsRequired();
            entity.Property(e => e.VectorId).IsRequired();
            entity.Property(e => e.ChunkOrder).IsRequired();
            entity.Property(e => e.TokenCount).IsRequired();
            entity.Property(e => e.ChunkSize).IsRequired();
        });

        modelBuilder.Entity<EmbeddingModel>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired();
            entity.Property(e => e.Provider).IsRequired();
            entity.Property(e => e.Description).IsRequired(false);
            entity.Property(e => e.Dimensions).IsRequired();
            entity.Property(e => e.MaxTokens).IsRequired();
            entity.Property(e => e.IsActive).IsRequired();
        });

        modelBuilder.Entity<EmbeddingTag>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired();
            entity.Property(e => e.Description).IsRequired(false);

            // Add unique constraint on tag name
            entity.HasIndex(e => e.Name).IsUnique();
        });

        modelBuilder.Entity<EmbeddingCollection>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired();
            entity.Property(e => e.Description).IsRequired(false);

            // Add unique constraint on collection name
            entity.HasIndex(e => e.Name).IsUnique();
        });

        // Configure BaseEntity properties for all entities
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            if (typeof(BaseEntity).IsAssignableFrom(entityType.ClrType))
            {
                modelBuilder.Entity(entityType.ClrType)
                    .Property(nameof(BaseEntity.CreatedAt))
                    .IsRequired();

                modelBuilder.Entity(entityType.ClrType)
                    .Property(nameof(BaseEntity.LastModifiedAt))
                    .IsRequired();

                modelBuilder.Entity(entityType.ClrType)
                    .Property(nameof(BaseEntity.CreatedBy))
                    .IsRequired(false);

                modelBuilder.Entity(entityType.ClrType)
                    .Property(nameof(BaseEntity.LastModifiedBy))
                    .IsRequired(false);
            }
        }

        // Apply global query filter for soft delete
        modelBuilder.Entity<EmbeddingDocument>().HasQueryFilter(e => !e.IsDeleted);
        modelBuilder.Entity<EmbeddingChunk>().HasQueryFilter(e => !e.IsDeleted);
        modelBuilder.Entity<EmbeddingModel>().HasQueryFilter(e => !e.IsDeleted);
        modelBuilder.Entity<EmbeddingTag>().HasQueryFilter(e => !e.IsDeleted);
        modelBuilder.Entity<EmbeddingCollection>().HasQueryFilter(e => !e.IsDeleted);

        base.OnModelCreating(modelBuilder);
    }
}
