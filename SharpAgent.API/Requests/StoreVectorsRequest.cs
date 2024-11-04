namespace SharpAgent.API.Requests;

public class StoreVectorsRequest
{
    public required List<float[]> Vectors { get; init; }
    public required string Namespace { get; init; }
    public Dictionary<string, string> Metadata { get; init; } = new();
    public string? FileName { get; init; }
}
