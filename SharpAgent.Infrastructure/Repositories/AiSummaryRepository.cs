using Microsoft.EntityFrameworkCore;
using SharpAgent.Application.IRepositories;
using SharpAgent.Domain.Entities;
using SharpAgent.Infrastructure.Data;

namespace SharpAgent.Infrastructure.Repositories;

public class AiSummaryRepository : Repository<AiSummary>, IAiSummaryRepository
{
    private readonly AppDbContext _context;

    public AiSummaryRepository(AppDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<IEnumerable<AiSummary>> GetByVideoId(int videoId)
    {
        return await _context.AiSummaries
            .Where(s => s.VideoId == videoId && !s.IsDeleted)
            .OrderByDescending(s => s.CreatedAt)
            .ToListAsync();
    }

    public async Task<AiSummary?> GetMostRecentByVideoId(int videoId)
    {
        return await _context.AiSummaries
            .Where(s => s.VideoId == videoId && !s.IsDeleted)
            .OrderByDescending(s => s.CreatedAt)
            .FirstOrDefaultAsync();
    }
}
