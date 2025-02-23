using MediatR;

namespace SharpAgent.Application.Categories.Commands.Delete;

public class DeleteCategoryCommand : IRequest<bool>
{
    public Guid Id { get; set; }
}