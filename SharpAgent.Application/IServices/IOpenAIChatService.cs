using SharpAgent.Domain.Models;

namespace SharpAgent.Application.IServices;

public interface IOpenAIChatService
{
    /// <summary>
    /// Sends a chat completion request to OpenAI
    /// </summary>
    /// <param name="content">The message content</param>
    /// <param name="role">Optional role (user, system, or assistant). Defaults to 'user' if not specified.</param>
    /// <returns>The chat completion response</returns>
    Task<ChatResponse> SendChatCompletionAsync(
        string content,
        string? role = null);

    /// <summary>
    /// Generates a summary of the provided text using the OpenAI chat completion API
    /// </summary>
    /// <param name="text">The text to summarize</param>
    /// <param name="maxLength">The maximum length of the summary in tokens (optional)</param>
    /// <returns>A string containing the summary</returns>
    Task<string> GenerateSummaryAsync(string text, int? maxLength = null);
}
