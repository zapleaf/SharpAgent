using SharpAgent.Domain.Common;

namespace SharpAgent.Domain.Entities;

public class EmbeddingModel : BaseEntity
{
    public EmbeddingModel()
    {
        Documents = new List<EmbeddingDocument>();
    }

    public required string Name { get; set; }
    public required string Provider { get; set; }
    public string? Description { get; set; }
    public int Dimensions { get; set; }
    public int MaxTokens { get; set; }
    public bool IsActive { get; set; }

    // Navigation property
    public ICollection<EmbeddingDocument> Documents { get; set; }
}
