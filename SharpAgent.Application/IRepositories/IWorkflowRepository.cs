using SharpAgent.Application.Models;
using SharpAgent.Domain.Entities;

namespace SharpAgent.Application.IRepositories;

public interface IWorkflowRepository
{
    Task<AgentWorkflow> StartWorkflow(Guid workflowId);
    Task UpdateWorkflowStep(Guid workflowId, int stepOrder, object result);
    Task<WorkflowResult> CompleteWorkflow(AgentWorkflow workflow);
}
