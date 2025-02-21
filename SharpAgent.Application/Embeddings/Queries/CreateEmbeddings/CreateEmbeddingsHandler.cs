using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;
using SharpAgent.Application.IServices;

namespace SharpAgent.Application.Embeddings.Queries.CreateEmbeddings;

public class CreateEmbeddingsHandler
    : IRequestHandler<CreateEmbeddingsQuery, CreateEmbeddingsResponse>
{
    private readonly IEmbeddingService _embeddingService;
    private readonly IMapper _mapper;
    private readonly ILogger<CreateEmbeddingsHandler> _logger;

    public CreateEmbeddingsHandler(
        IEmbeddingService embeddingService,
        IMapper mapper,
        ILogger<CreateEmbeddingsHandler> logger)
    {
        _embeddingService = embeddingService;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<CreateEmbeddingsResponse> Handle(CreateEmbeddingsQuery request, CancellationToken ct)
    {
        try
        {
            _logger.LogInformation($"Creating embeddings for {request.TextSections.Count} sections");

            var embeddings = await _embeddingService.CreateEmbeddingsAsync(request.TextSections);

            return new CreateEmbeddingsResponse
            {
                Embeddings = embeddings
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating embeddings");
            throw;
        }
    }
}
