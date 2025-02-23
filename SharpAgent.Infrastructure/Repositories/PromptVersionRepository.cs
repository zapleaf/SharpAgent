using Microsoft.EntityFrameworkCore;

using SharpAgent.Application.IRepositories;
using SharpAgent.Domain.Entities;
using SharpAgent.Infrastructure.Data;

namespace SharpAgent.Infrastructure.Repositories;

public class PromptVersionRepository : Repository<PromptVersion>, IPromptVersionRepository
{
    private readonly AppDbContext _context;

    public PromptVersionRepository(AppDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<PromptVersion?> GetByCodeAndVersion(string code, string version)
    {
        return await _context.PromptVersions
            .FirstOrDefaultAsync(p => p.Code == code && p.Version == version);
    }

    public async Task<IEnumerable<PromptVersion>> GetLatestVersions()
    {
        // Get the latest version for each code
        var latestVersions = await _context.PromptVersions
            .Where(p => !p.IsDeleted)
            .GroupBy(p => p.Code)
            .Select(g => g.OrderByDescending(p => p.Version)
                         .ThenByDescending(p => p.CreatedAt)
                         .First())
            .ToListAsync();

        return latestVersions;
    }

    public async Task<PromptVersion?> GetLatestByCode(string code)
    {
        return await _context.PromptVersions
            .Where(p => p.Code == code && !p.IsDeleted)
            .OrderByDescending(p => p.Version)
            .ThenByDescending(p => p.CreatedAt)
            .FirstOrDefaultAsync();
    }
}
