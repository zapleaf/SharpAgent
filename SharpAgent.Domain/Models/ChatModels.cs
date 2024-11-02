namespace SharpAgent.Domain.Models;

public class ChatResponse
{
    public required string Id { get; init; }
    public required string Object { get; init; }
    public required long Created { get; init; }
    public required string Model { get; init; }
    public required List<ChatContent> Choices { get; init; }
    public required int PromptTokens { get; init; }
    public required int CompletionTokens { get; init; }
    public required int TotalTokens { get; init; }
    public required string SystemFingerprint { get; init; }
}

public class ChatContent
{
    public required int Index { get; init; }
    public required string Role { get; init; }
    public required string Content { get; init; }
    public string? Refusal { get; init; }
    public object? LogProbs { get; init; }
    public required string FinishReason { get; init; }
}
