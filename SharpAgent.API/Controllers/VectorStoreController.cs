using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharpAgent.API.Requests;
using SharpAgent.Application.VectorStore.Commands.StoreVectors;
using SharpAgent.Domain.Models;

namespace SharpAgent.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class VectorStoreController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<VectorStoreController> _logger;

    public VectorStoreController(
        IMediator mediator,
        ILogger<VectorStoreController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    [HttpPost("store")]
    [ProducesResponseType(typeof(StoreVectorsResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<StoreVectorsResponse>> StoreVectors(
        [FromBody] StoreVectorsRequest request,
        CancellationToken cancellationToken)
    {
        try
        {
            var vectors = request.Vectors.Select((values, index) => new EmbeddingVector
            {
                Id = Guid.NewGuid(),
                Values = values,
                FileName = request.FileName,
                Metadata = new Dictionary<string, string>(request.Metadata)
                {
                    ["index"] = index.ToString()
                }
            }).ToList();

            var command = new StoreVectorsCommand
            {
                Vectors = vectors,
                Namespace = request.Namespace
            };

            var response = await _mediator.Send(command, cancellationToken);

            return response.Success ?
                Ok(response) :
                BadRequest(new ProblemDetails
                {
                    Title = "Vector Storage Failed",
                    Detail = response.Error
                });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error processing vector storage request");
            throw;
        }
    }
}
