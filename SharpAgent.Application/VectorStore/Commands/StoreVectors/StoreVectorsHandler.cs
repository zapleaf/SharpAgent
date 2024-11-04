using MediatR;
using Microsoft.Extensions.Logging;
using SharpAgent.Application.IServices;

namespace SharpAgent.Application.VectorStore.Commands.StoreVectors;

public class StoreVectorsHandler : IRequestHandler<StoreVectorsCommand, StoreVectorsResponse>
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

    public async Task<StoreVectorsResponse> Handle(
        StoreVectorsCommand request,
        CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation(
                "Storing {Count} vectors in namespace {Namespace}",
                request.Vectors.Count,
                request.Namespace);

            var addCount = await _vectorDbService.UpsertVectors(request.Vectors, request.Namespace);

            _logger.LogInformation(
                "Successfully stored {Count} vectors in namespace {Namespace}",
                request.Vectors.Count,
                request.Namespace);

            return StoreVectorsResponse.Successful(addCount);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,
                "Error storing vectors in namespace {Namespace}: {Message}",
                request.Namespace,
                ex.Message);

            return StoreVectorsResponse.Failed(ex.Message);
        }
    }
}
