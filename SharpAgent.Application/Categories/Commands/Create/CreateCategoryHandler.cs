using AutoMapper;
using SharpAgent.Application.Categories.Queries.GetAll;
using SharpAgent.Application.IRepositories;
using SharpAgent.Domain.Entities;
using MediatR;

namespace SharpAgent.Application.Categories.Commands.Create;

public class CreateCategoryHandler : IRequestHandler<CreateCategoryCommand, CategoryResponse>
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly IMapper _mapper;

    public CreateCategoryHandler(ICategoryRepository categoryRepository, IMapper mapper)
    {
        _categoryRepository = categoryRepository;
        _mapper = mapper;
    }

    public async Task<CategoryResponse> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
    {
        var category = _mapper.Map<Category>(request.Category);
        var result = await _categoryRepository.Create(category);
        return _mapper.Map<CategoryResponse>(result);
    }
}