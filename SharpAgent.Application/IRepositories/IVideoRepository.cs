using SharpAgent.Domain.Entities;

namespace SharpAgent.Application.IRepositories;

public interface IVideoRepository : IRepository<Video>
{
    Task<int> Create(List<Video> videos);
    Task<Video> Get(string ytid, bool includeStats = false);
    Task<IEnumerable<Video>> GetByChannel(string ytChannelID);
    Task<IEnumerable<Video>> GetByChannel(Guid channelID);
    Task<IEnumerable<Video>> GetByCategory(Guid categoryId);
    Task<int> Update(List<Video> videos);
}
