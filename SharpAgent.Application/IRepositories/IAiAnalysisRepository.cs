using SharpAgent.Domain.Entities;

namespace SharpAgent.Application.IRepositories;

public interface IAiAnalysisRepository : IRepository<AiAnalysis>
{
    Task<IEnumerable<AiAnalysis>> GetByChannelId(int channelId);
    Task<AiAnalysis?> GetMostRecentByChannelId(int channelId);
}
