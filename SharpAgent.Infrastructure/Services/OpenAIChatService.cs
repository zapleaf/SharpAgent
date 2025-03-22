using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

using OpenAI;
using OpenAI.Chat;

using SharpAgent.Application.IServices;
using SharpAgent.Domain.Models;
using System.ClientModel;
using System.Text.Json;

namespace SharpAgent.Infrastructure.Services;

public class OpenAIChatService : IOpenAIChatService
{
    private readonly ChatClient _chatClient;
    private readonly OpenAIClient _client;
    private readonly ILogger<OpenAIChatService> _logger;
    private readonly string _defaultModel;

    public OpenAIChatService(
        IConfiguration configuration,
        ILogger<OpenAIChatService> logger)
    {
        if (configuration == null)
            throw new ArgumentNullException(nameof(configuration));

        _logger = logger ?? throw new ArgumentNullException(nameof(logger));

        var apiKey = configuration["OpenAI:ApiKey"]
            ?? throw new InvalidOperationException("OpenAI:ApiKey configuration is missing");

        _defaultModel = configuration["OpenAI:ChatModel"]
            ?? throw new InvalidOperationException("OpenAI:ChatModel configuration is missing");

        _client = new(apiKey);
        _chatClient = _client.GetChatClient(_defaultModel);
    }

    public async Task<ChatResponse> SendChatCompletionAsync(string content, string? role = null)
    {
        try
        {
            List<ChatMessage> messages = new();
            messages.Add(ChatMessage.CreateUserMessage(content));

            var options = new ChatCompletionOptions
            {
                Temperature = 0.7f,  // Default settings, could be moved to configuration
                MaxOutputTokenCount = 2000
            };

            ClientResult result = await _chatClient.CompleteChatAsync(messages, options);

            var jsonResponse = result.GetRawResponse().Content.ToString();
            using JsonDocument doc = JsonDocument.Parse(jsonResponse);
            var root = doc.RootElement;

            // Parse the complete response
            var response = new ChatResponse
            {
                Id = root.GetProperty("id").GetString()!,
                Object = root.GetProperty("object").GetString()!,
                Created = root.GetProperty("created").GetInt64(),
                Model = root.GetProperty("model").GetString()!,
                SystemFingerprint = root.GetProperty("system_fingerprint").GetString()!,
                Choices = ParseChoices(root.GetProperty("choices")),
                PromptTokens = root.GetProperty("usage").GetProperty("prompt_tokens").GetInt32(),
                CompletionTokens = root.GetProperty("usage").GetProperty("completion_tokens").GetInt32(),
                TotalTokens = root.GetProperty("usage").GetProperty("total_tokens").GetInt32()
            };

            return response;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error sending chat completion request");
            throw;
        }
    }

    public async Task<string> GenerateSummaryAsync(string text, int? maxLength = null)
    {
        try
        {
            // Create a system message that instructs the model to generate a one-paragraph summary
            List<ChatMessage> messages = new();

            // Add system message with instructions
            messages.Add(ChatMessage.CreateSystemMessage(
                "You are a helpful assistant that creates concise, informative summaries. " +
                "Provide a one paragraph summary that captures the key points of the text."));

            // Add the text to summarize as a user message
            messages.Add(ChatMessage.CreateUserMessage(text));

            // Configure the options
            var options = new ChatCompletionOptions
            {
                Temperature = 0.3f,  // Lower temperature for more focused responses
                MaxOutputTokenCount = maxLength ?? 1000
            };

            // Send the request
            ClientResult result = await _chatClient.CompleteChatAsync(messages, options);

            // Extract the summary from the response
            var jsonResponse = result.GetRawResponse().Content.ToString();
            using JsonDocument doc = JsonDocument.Parse(jsonResponse);
            var root = doc.RootElement;
            var choices = root.GetProperty("choices");

            if (choices.GetArrayLength() > 0)
            {
                var firstChoice = choices[0];
                var message = firstChoice.GetProperty("message");
                return message.GetProperty("content").GetString() ?? string.Empty;
            }

            return string.Empty;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error generating summary");
            throw;
        }
    }

    private static List<ChatContent> ParseChoices(JsonElement choicesElement)
    {
        var choices = new List<ChatContent>();
        foreach (var choice in choicesElement.EnumerateArray())
        {
            var message = choice.GetProperty("message");
            choices.Add(new ChatContent
            {
                Index = choice.GetProperty("index").GetInt32(),
                Role = message.GetProperty("role").GetString()!,
                Content = message.GetProperty("content").GetString()!,
                Refusal = message.TryGetProperty("refusal", out var refusal) ?
                    refusal.GetString() : null,
                LogProbs = choice.TryGetProperty("logprobs", out var logprobs) &&
                    !logprobs.ValueKind.Equals(JsonValueKind.Null) ?
                    JsonSerializer.Deserialize<object>(logprobs) : null,
                FinishReason = choice.GetProperty("finish_reason").GetString()!
            });
        }
        return choices;
    }
}