namespace SharpAgent.Domain.Models;

public class EmbeddingVector
{
    public Guid Id { get; set; }
    public required float[] Values { get; set; }
    public Dictionary<string, string> Metadata { get; set; } = new();
    public string? FileName { get; set; }  // Optional, depending on your needs
}
