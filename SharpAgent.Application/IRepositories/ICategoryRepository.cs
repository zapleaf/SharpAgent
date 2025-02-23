using SharpAgent.Domain.Entities;

namespace SharpAgent.Application.IRepositories;

public interface ICategoryRepository : IRepository<Category>
{
    Task<List<Category>> GetAllWithChildren();
    Task<Category> GetByIdWithChildren(Guid id);
}
