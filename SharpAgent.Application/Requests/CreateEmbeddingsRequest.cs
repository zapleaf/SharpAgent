namespace SharpAgent.API.Requests;

public class CreateEmbeddingsRequest
{
    public required List<string> TextSections { get; init; }
}
