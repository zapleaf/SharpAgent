using MediatR;
using Microsoft.Extensions.Logging;
using SharpAgent.Application.Commands;
using SharpAgent.Application.IServices;
using SharpAgent.Application.Models;

namespace SharpAgent.Application.Handlers;

public class CreateEmbeddingsHandler : IRequestHandler<CreateEmbeddingsCommand, CreateEmbeddingsResult>
{
    private readonly IEmbeddingService _embeddingService;
    private readonly ILogger<CreateEmbeddingsHandler> _logger;

    public CreateEmbeddingsHandler(
        IEmbeddingService embeddingService,
        ILogger<CreateEmbeddingsHandler> logger)
    {
        _embeddingService = embeddingService;
        _logger = logger;
    }

    public async Task<CreateEmbeddingsResult> Handle(CreateEmbeddingsCommand command, CancellationToken ct)
    {
        _logger.LogInformation($"Creating embeddings for {command.TextSections.Count} sections");
        var embeddings = await _embeddingService.CreateEmbeddingsAsync(command.TextSections);
        return new CreateEmbeddingsResult(embeddings);
    }
}
