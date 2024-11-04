using MediatR;
using SharpAgent.Domain.Models;

namespace SharpAgent.Application.VectorStore.Commands.StoreVectors;

public class StoreVectorsCommand : IRequest<StoreVectorsResponse>
{
    public required List<EmbeddingVector> Vectors { get; init; }
    public required string Namespace { get; init; }
}
