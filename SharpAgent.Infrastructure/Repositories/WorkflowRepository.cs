using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using SharpAgent.Application.IRepositories;
using SharpAgent.Application.Models;
using SharpAgent.Domain.Entities;
using SharpAgent.Infrastructure.Data;

namespace SharpAgent.Infrastructure.Repositories;

public class WorkflowRepository : IWorkflowRepository
{
    private readonly AppDbContext _context;
    private readonly ILogger<WorkflowRepository> _logger;

    public WorkflowRepository(
        AppDbContext context,
        ILogger<WorkflowRepository> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<AgentWorkflow> StartWorkflow(Guid workflowId)
    {
        var workflow = await _context.Set<AgentWorkflow>()
            .Include(w => w.Tasks)
            .Include(w => w.Agents)  // Include agents
            .FirstOrDefaultAsync(w => w.Id == workflowId)
            ?? throw new InvalidOperationException($"Workflow {workflowId} not found");

        workflow.Status = Domain.Enums.WorkflowStatus.Running;
        workflow.StartedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return workflow;
    }

    public async Task UpdateWorkflowStep(Guid workflowId, int stepOrder, object result)
    {
        var workflow = await _context.Set<AgentWorkflow>()
            .Include(w => w.Tasks)
            .Include(w => w.Agents)  // Include agents
            .FirstOrDefaultAsync(w => w.Id == workflowId)
            ?? throw new InvalidOperationException($"Workflow {workflowId} not found");

        // Get the agent for this step from the workflow configuration
        var workflowSteps = System.Text.Json.JsonSerializer.Deserialize<WorkflowSteps>(workflow.WorkflowSteps ?? "");
        var currentStep = workflowSteps?.Steps.FirstOrDefault(s => s.Order == stepOrder)
            ?? throw new InvalidOperationException($"Step {stepOrder} not found in workflow configuration");

        // Find the corresponding agent for this step
        var agent = workflow.Agents.FirstOrDefault(a => a.Role == currentStep.AgentRole)
            ?? throw new InvalidOperationException($"No agent found with role {currentStep.AgentRole}");

        workflow.CurrentStep = stepOrder;

        // Create task record with required Agent property
        var task = new AgentTask
        {
            WorkflowId = workflowId,
            Name = $"Step {stepOrder}",
            Description = "Workflow step execution",
            ServiceType = result.GetType().Name,
            ResultJson = System.Text.Json.JsonSerializer.Serialize(result),
            Status = Domain.Enums.TaskStatus.Completed,  // Fully qualified enum
            Agent = agent,  // Set the required Agent
            AgentId = agent.Id
        };

        workflow.Tasks.Add(task);
        await _context.SaveChangesAsync();
    }

    public async Task<WorkflowResult> CompleteWorkflow(AgentWorkflow workflow)
    {
        workflow.Status = Domain.Enums.WorkflowStatus.Completed;
        workflow.CompletedAt = DateTime.UtcNow;

        await _context.SaveChangesAsync();

        return new WorkflowResult(true);
    }
}
