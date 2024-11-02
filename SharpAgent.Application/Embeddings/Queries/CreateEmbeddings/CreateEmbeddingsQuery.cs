using MediatR;

namespace SharpAgent.Application.Embeddings.Queries.CreateEmbeddings;

public record CreateEmbeddingsQuery : IRequest<CreateEmbeddingsResponse>
{
    public required List<string> TextSections { get; init; }
}
