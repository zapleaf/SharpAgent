namespace SharpAgent.API.Model.Chat;

public class ChatCompletionRequest
{
    public required string Content { get; init; }
    public string? Role { get; init; }
}

