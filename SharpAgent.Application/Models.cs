using SharpAgent.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpAgent.Application.Models;

public record ProcessDocumentResult(ProcessedDocument Document);
public record CreateEmbeddingsResult(List<ReadOnlyMemory<float>> Embeddings);
public record StoreVectorResult(bool Success);
public record WorkflowResult(bool Success, string? Error = null);

public class WorkflowSteps
{
    public List<WorkflowStep> Steps { get; set; } = new();
}

public class WorkflowStep
{
    public int Order { get; set; }
    public required string ServiceType { get; set; }
    public required string AgentRole { get; set; }
    public Dictionary<string, object> Parameters { get; set; } = new();
}
