using SharpAgent.Domain.Entities;

namespace SharpAgent.Application.IRepositories;

public interface IChannelRepository : IRepository<Channel>
{
    Task<IEnumerable<Channel>> GetWithCategories();
    Task<IEnumerable<Channel>> GetWithCategoriesAndVideos();
    Task<IEnumerable<Channel>> GetByCategory(Guid categoryId);
    Task<Channel> Get(string YTID);
    Task<bool> AddCategoryToChannel(Guid categoryId, Guid channelId);
    Task<bool> RemoveCategoryFromChannel(Guid categoryId, Guid channelId);
}
