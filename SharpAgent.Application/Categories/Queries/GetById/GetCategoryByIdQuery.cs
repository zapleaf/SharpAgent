using MediatR;
using SharpAgent.Domain.Entities;

namespace SharpAgent.Application.Categories.Queries.GetById;

public class GetCategoryByIdQuery : IRequest<Category>
{
    public Guid Id { get; set; }
}
