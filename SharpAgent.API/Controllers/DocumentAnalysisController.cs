using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharpAgent.API.Requests;
using SharpAgent.Application.DocumentAnalysis.Queries.ExtractParagraphs;

namespace SharpAgent.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class DocumentAnalysisController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<DocumentAnalysisController> _logger;

        public DocumentAnalysisController(IMediator mediator, ILogger<DocumentAnalysisController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpPost("extractparagraphs")]
        [ProducesResponseType(typeof(ExtractParagraphsResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ExtractParagraphsResponse>> ExtractParagraphs([FromBody] ExtractParagraphsRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var query = new ExtractParagraphsQuery
                {
                    DocumentUrl = request.DocumentUrl
                };

                var response = await _mediator.Send(query, cancellationToken);
                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing document analysis request");
                throw;
            }
        }
    }
}
