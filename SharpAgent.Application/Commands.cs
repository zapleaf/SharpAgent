using MediatR;
using SharpAgent.Application.Models;
using SharpAgent.Domain.Attributes;

namespace SharpAgent.Application.Commands;

[ServiceCommand("Embedding")]
public record CreateEmbeddingsCommand : IRequest<CreateEmbeddingsResult>
{
    public List<string> TextSections { get; init; }
    public Guid WorkflowId { get; init; }
}

[ServiceCommand("VectorDb")]
public record StoreVectorsCommand : IRequest<StoreVectorResult>
{
    public List<ReadOnlyMemory<float>> Vectors { get; init; }
    public Guid WorkflowId { get; init; }
}

public record StartWorkflowCommand : IRequest<WorkflowResult>
{
    public Guid WorkflowId { get; init; }
    public object? InputParameters { get; init; }
}