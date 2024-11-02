
using SharpAgent.Domain.Models;

namespace SharpAgent.Application.ChatCompletion.Queries.SendChatCompletion;

public class SendChatCompletionResponse
{
    public required string Id { get; init; }
    public required string Model { get; init; }
    public required List<ChatContent> Choices { get; init; }
    public required int PromptTokens { get; init; }
    public required int CompletionTokens { get; init; }
    public required int TotalTokens { get; init; }
}
