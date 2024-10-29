using SharpAgent.Domain.Common;

namespace SharpAgent.Domain.Entities;

public class EmbeddingChunk : BaseEntity
{
    public Guid DocumentId { get; set; }
    public required string ChunkText { get; set; }
    public int ChunkOrder { get; set; }
    public required string VectorId { get; set; }
    public int TokenCount { get; set; }
    public int ChunkSize { get; set; }

    // Navigation property
    public required EmbeddingDocument Document { get; set; }
}
