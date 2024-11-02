
namespace SharpAgent.Application.Embeddings.Queries.CreateEmbeddings;

public class CreateEmbeddingsResponse
{
    public required List<ReadOnlyMemory<float>> Embeddings { get; init; }
}
