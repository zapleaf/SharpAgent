using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharpAgent.Application.Commands;

namespace SharpAgent.API.Controllers;

public class WorkFlowController : Controller
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class WorkflowController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<WorkflowController> _logger;

        public WorkflowController(IMediator mediator, ILogger<WorkflowController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }

        [HttpPost("{workflowId}")]
        public async Task<IActionResult> ExecuteWorkflow(Guid workflowId, [FromBody] object? parameters = null)
        {
            try
            {
                _logger.LogInformation($"Starting workflow {workflowId}");

                var command = new StartWorkflowCommand
                {
                    WorkflowId = workflowId,
                    InputParameters = parameters
                };

                var result = await _mediator.Send(command);

                return result.Success
                    ? Ok(result)
                    : BadRequest(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error executing workflow");
                return StatusCode(500, new { error = "Internal server error" });
            }
        }
    }
}
