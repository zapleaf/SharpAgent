using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharpAgent.API.Requests;
using SharpAgent.Application.Embeddings.Queries.CreateEmbeddings;

namespace SharpAgent.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class EmbeddingsController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<EmbeddingsController> _logger;

    public EmbeddingsController(IMediator mediator, ILogger<EmbeddingsController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    [HttpPost("create")]
    [ProducesResponseType(typeof(CreateEmbeddingsResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<CreateEmbeddingsResponse>> CreateEmbeddings([FromBody] CreateEmbeddingsRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var query = new CreateEmbeddingsQuery
            {
                TextSections = request.TextSections
            };

            var response = await _mediator.Send(query, cancellationToken);
            return Ok(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error processing embeddings request");
            throw;
        }
    }
}
