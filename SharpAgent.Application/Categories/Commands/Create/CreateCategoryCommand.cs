using SharpAgent.Application.Categories.Queries.GetAll;
using MediatR;

namespace SharpAgent.Application.Categories.Commands.Create;

public class CreateCategoryCommand : IRequest<CategoryResponse>
{
    public CreateCategoryRequest Category { get; set; } = null!;
}