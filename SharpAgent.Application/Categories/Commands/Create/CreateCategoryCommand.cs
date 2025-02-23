using SharpAgent.Application.Categories.Queries.GetAll;
using MediatR;

namespace SharpAgent.Application.Categories.Commands.Create;

public class CreateCategoryCommand : IRequest<CategoryDto>
{
    public CreateCategoryRequest Category { get; set; } = null!;
}