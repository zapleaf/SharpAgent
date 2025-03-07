namespace SharpAgent.API.Requests;

public class ChatCompletionRequest
{
    public required string Content { get; init; }
    public string? Role { get; init; }
}

