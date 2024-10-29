using SharpAgent.Domain.Common;

namespace SharpAgent.Domain.Entities;

public class EmbeddingTag : BaseEntity
{
    public EmbeddingTag()
    {
        Documents = new List<EmbeddingDocument>();
    }

    public required string Name { get; set; }
    public string? Description { get; set; }

    // Navigation property
    public ICollection<EmbeddingDocument> Documents { get; set; }
}
