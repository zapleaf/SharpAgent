using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

using SharpAgent.Application.IServices;

using OpenAI.Embeddings;

namespace SharpAgent.Infrastructure.Services;

internal class OpenAIEmbeddingService : IEmbeddingService
{
    private readonly EmbeddingClient _client;
    private readonly ILogger<OpenAIEmbeddingService> _logger;

    public OpenAIEmbeddingService(IConfiguration configuration,
        ILogger<OpenAIEmbeddingService> logger)
    {
        if (configuration == null)
            throw new ArgumentNullException(nameof(configuration));

        _logger = logger ?? throw new ArgumentNullException(nameof(logger));

        var embeddingModel = configuration["OpenAI:EmbeddingModel"]
            ?? throw new InvalidOperationException("OpenAI:EmbeddingModel configuration is missing");
        var apiKey = configuration["OpenAI:ApiKey"]
            ?? throw new InvalidOperationException("OpenAI:ApiKey configuration is missing");

        _client = new EmbeddingClient(model: embeddingModel, apiKey);
    }

    public async Task<List<ReadOnlyMemory<float>>> CreateEmbeddingsAsync(List<string> inputs)
    {
        try
        {
            if (inputs == null || inputs.Count == 0)
            {
                throw new ArgumentException("Inputs cannot be null or empty", nameof(inputs));
            }

            _logger.LogInformation("Generating embeddings for {Count} inputs", inputs.Count);

            OpenAIEmbeddingCollection embeddings = (await _client.GenerateEmbeddingsAsync(inputs)).Value;

            // Convert each of the OpenAI Embeddings into something more generic
            List<ReadOnlyMemory<float>> vectors = new();
            foreach (var embedding in embeddings)
            {
                vectors.Add(embedding.ToFloats());
            }

            _logger.LogInformation("Successfully generated {Count} embeddings", vectors.Count);

            return vectors;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error generating embeddings: {Message}", ex.Message);
            throw;
        }
    }
}
