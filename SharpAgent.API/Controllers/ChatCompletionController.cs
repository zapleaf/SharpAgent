using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharpAgent.API.Requests;
using SharpAgent.Application.ChatCompletion.Queries.SendChatCompletion;

namespace SharpAgent.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ChatCompletionController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly ILogger<ChatCompletionController> _logger;

    public ChatCompletionController(IMediator mediator, ILogger<ChatCompletionController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }

    [HttpPost("sendchatcompletion")]
    [ProducesResponseType(typeof(SendChatCompletionResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<SendChatCompletionResponse>> SendChatCompletion([FromBody] ChatCompletionRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var query = new SendChatCompletionQuery
            {
                Content = request.Content,
                Role = request.Role
            };

            var response = await _mediator.Send(query, cancellationToken);
            return Ok(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error processing chat completion request");
            throw;
        }
    }
}
