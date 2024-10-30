using MediatR;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using System.Reflection;
using SharpAgent.Application.IRepositories;
using SharpAgent.Application.Models;
using SharpAgent.Application.Commands;
using SharpAgent.Domain.Attributes;

namespace SharpAgent.Application.Handlers;

public class WorkflowHandler : IRequestHandler<StartWorkflowCommand, WorkflowResult>
{
    private readonly IMediator _mediator;
    private readonly IWorkflowRepository _workflowRepository;
    private readonly ILogger<WorkflowHandler> _logger;
    private readonly IDictionary<string, Type> _commandTypeMap;

    public WorkflowHandler(
        IMediator mediator,
        IWorkflowRepository workflowRepository,
        ILogger<WorkflowHandler> logger)
    {
        _mediator = mediator;
        _workflowRepository = workflowRepository;
        _logger = logger;
        _commandTypeMap = DiscoverCommandTypes();
    }

    private IDictionary<string, Type> DiscoverCommandTypes()
    {
        var commandTypes = AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(a => a.GetTypes())
            .Where(t => t.GetInterfaces()
                .Any(i => i.IsGenericType &&
                     i.GetGenericTypeDefinition() == typeof(IRequest<>)));

        foreach (var type in commandTypes)
        {
            var attribute = type.GetCustomAttribute<ServiceCommandAttribute>();
            if (attribute != null)
            {
                _logger.LogInformation($"Found command: {type.Name} for service: {attribute.ServiceType}");
            }
        }

        return commandTypes
            .Where(t => t.GetCustomAttribute<ServiceCommandAttribute>() != null)
            .ToDictionary(
                t => t.GetCustomAttribute<ServiceCommandAttribute>()!.ServiceType,
                t => t
            );
    }

    public async Task<WorkflowResult> Handle(StartWorkflowCommand command, CancellationToken ct)
    {
        try
        {
            var workflow = await _workflowRepository.StartWorkflow(command.WorkflowId);
            var workflowSteps = JsonSerializer.Deserialize<WorkflowSteps>(workflow.WorkflowSteps);

            if (workflowSteps?.Steps == null || !workflowSteps.Steps.Any())
            {
                throw new InvalidOperationException("Workflow has no steps defined");
            }

            object previousResult = command.InputParameters ?? new { };

            foreach (var step in workflowSteps.Steps.OrderBy(s => s.Order))
            {
                _logger.LogInformation($"Executing step {step.Order}: {step.ServiceType}");

                if (!_commandTypeMap.TryGetValue(step.ServiceType, out Type? commandType))
                {
                    throw new InvalidOperationException($"Unknown service type: {step.ServiceType}");
                }

                var stepCommand = CreateCommand(commandType, previousResult, workflow.Id);
                previousResult = await _mediator.Send(stepCommand, ct);

                await _workflowRepository.UpdateWorkflowStep(workflow.Id, step.Order, previousResult);
            }

            return await _workflowRepository.CompleteWorkflow(workflow);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Workflow execution failed");
            return new WorkflowResult(false, ex.Message);
        }
    }

    private object CreateCommand(Type commandType, object previousResult, Guid workflowId)
    {
        var command = Activator.CreateInstance(commandType);
        var properties = commandType.GetProperties();

        foreach (var prop in properties)
        {
            if (prop.Name == "WorkflowId")
            {
                prop.SetValue(command, workflowId);
                continue;
            }

            var previousResultProp = previousResult.GetType().GetProperty(prop.Name);
            if (previousResultProp != null)
            {
                var value = previousResultProp.GetValue(previousResult);
                prop.SetValue(command, value);
            }
        }

        return command;
    }
}
