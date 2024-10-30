using MediatR;
using Microsoft.Extensions.Logging;
using SharpAgent.Application.IServices;
using SharpAgent.Application.Models;
using SharpAgent.Application.Commands;

using SharpAgent.Domain.Models;

namespace SharpAgent.Application.Handlers;

public class StoreVectorsHandler : IRequestHandler<StoreVectorsCommand, StoreVectorResult>
{
    private readonly IVectorDbService _vectorDbService;
    private readonly ILogger<StoreVectorsHandler> _logger;

    public StoreVectorsHandler(
        IVectorDbService vectorDbService,
        ILogger<StoreVectorsHandler> logger)
    {
        _vectorDbService = vectorDbService;
        _logger = logger;
    }

    public async Task<StoreVectorResult> Handle(StoreVectorsCommand command, CancellationToken ct)
    {
        try
        {
            _logger.LogInformation($"Storing {command.Vectors.Count} vectors");

            // Convert vectors to EmbeddingVector format
            var embeddings = command.Vectors.Select((vector, index) => new EmbeddingVector
            {
                Id = Guid.NewGuid(),
                Values = vector.ToArray(),
                Metadata = new Dictionary<string, string>
                {
                    ["index"] = index.ToString()
                }
            }).ToList();

            await _vectorDbService.UpsertVectors(embeddings, "default");
            return new StoreVectorResult(true);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to store vectors");
            return new StoreVectorResult(false);
        }
    }
}
