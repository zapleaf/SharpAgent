using MediatR;

namespace SharpAgent.Application.Categories.Queries.GetAll;

public class GetAllCategoriesQuery : IRequest<List<CategoryResponse>>
{
}