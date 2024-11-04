using MediatR;
using SharpAgent.Application.Models;
using SharpAgent.Domain.Attributes;

namespace SharpAgent.Application.Commands;

public record StartWorkflowCommand : IRequest<WorkflowResult>
{
    public Guid WorkflowId { get; init; }
    public object? InputParameters { get; init; }
}