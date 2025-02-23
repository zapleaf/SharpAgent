using SharpAgent.Domain.Entities;

namespace SharpAgent.Application.IRepositories;

public interface IAiAnalysisRepository : IRepository<AiAnalysis>
{
    Task<IEnumerable<AiAnalysis>> GetByChannelId(Guid channelId);
    Task<AiAnalysis?> GetMostRecentByChannelId(Guid channelId);
}
