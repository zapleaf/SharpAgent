using AutoMapper;
using SharpAgent.Application.IRepositories;
using MediatR;

namespace SharpAgent.Application.Categories.Queries.GetAll;

public class GetAllCategoriesHandler : IRequestHandler<GetAllCategoriesQuery, List<CategoryDto>>
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly IMapper _mapper;

    public GetAllCategoriesHandler(ICategoryRepository categoryRepository, IMapper mapper)
    {
        _categoryRepository = categoryRepository;
        _mapper = mapper;
    }

    public async Task<List<CategoryDto>> Handle(GetAllCategoriesQuery request, CancellationToken cancellationToken)
    {
        var categories = await _categoryRepository.GetAllWithChildren();
        return _mapper.Map<List<CategoryDto>>(categories);
    }
}