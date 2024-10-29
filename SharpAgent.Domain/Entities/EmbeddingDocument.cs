using SharpAgent.Domain.Common;
using SharpAgent.Domain.Enums;

namespace SharpAgent.Domain.Entities;

public class EmbeddingDocument : BaseEntity
{
    public EmbeddingDocument()
    {
        Chunks = new List<EmbeddingChunk>();
        Tags = new List<EmbeddingTag>();
        Collections = new List<EmbeddingCollection>();
    }

    public required string Filename { get; set; }
    public required string Location { get; set; }
    public required string Namespace { get; set; }
    public required string IndexName { get; set; }
    public string? Description { get; set; }
    public DocumentStatus Status { get; set; }
    public DocumentType DocumentType { get; set; }
    public string? MetadataJson { get; set; }
    public long Size { get; set; }
    public Guid ModelId { get; set; }

    // Navigation properties
    public required EmbeddingModel Model { get; set; }
    public ICollection<EmbeddingChunk> Chunks { get; set; }
    public ICollection<EmbeddingTag> Tags { get; set; }
    public ICollection<EmbeddingCollection> Collections { get; set; }
}
