namespace SharpAgent.Application.Models;

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

public record ChatCompletionResult(
    string Content,
    string Role,
    int? PromptTokens,
    int? CompletionTokens,
    int? TotalTokens
);
