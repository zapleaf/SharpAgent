using SharpAgent.Domain.Common;

namespace SharpAgent.Domain.Entities;

public class EmbeddingCollection : BaseEntity
{
    public EmbeddingCollection()
    {
        Documents = new List<EmbeddingDocument>();
    }

    public required string Name { get; set; }
    public string? Description { get; set; }

    // Navigation property
    public ICollection<EmbeddingDocument> Documents { get; set; }
}
