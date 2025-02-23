using SharpAgent.Application.IRepositories;
using MediatR;

namespace SharpAgent.Application.Categories.Commands.Delete;

public class DeleteCategoryHandler : IRequestHandler<DeleteCategoryCommand, bool>
{
    private readonly ICategoryRepository _categoryRepository;

    public DeleteCategoryHandler(ICategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public async Task<bool> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
    {
        return await _categoryRepository.Delete(request.Id);
    }
}