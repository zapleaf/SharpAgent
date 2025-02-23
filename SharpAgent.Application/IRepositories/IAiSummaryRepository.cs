using SharpAgent.Domain.Entities;

namespace SharpAgent.Application.IRepositories;

public interface IAiSummaryRepository : IRepository<AiSummary>
{
    Task<IEnumerable<AiSummary>> GetByVideoId(Guid videoId);
    Task<AiSummary?> GetMostRecentByVideoId(Guid videoId);
}
