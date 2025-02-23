using SharpAgent.Application.IRepositories;
using SharpAgent.Infrastructure.Data;
using SharpAgent.Domain.Entities;

using Microsoft.EntityFrameworkCore;

namespace SharpAgent.Infrastructure.Repositories;

public class VideoRepository : Repository<Video>, IVideoRepository
{
    private readonly AppDbContext _context;

    public VideoRepository(AppDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<int> Create(List<Video> videos)
    {
        int createdCount = 0;
        foreach (var entityToCreate in videos)
        {
            var entity = await _context.Videos
                .Where(p => p.YTId == entityToCreate.YTId).FirstOrDefaultAsync();

            if (entity != null)
            {
                continue;
            }

            await _context.Videos.AddAsync(entityToCreate);
            createdCount++;
        }

        await _context.SaveChangesAsync();

        return createdCount;
    }

    public async Task<Video> Get(string ytid, bool includeStats = false)
    {
        var query = _context.Videos.Include(v => v.Channel);
        var entity = await query.Where(p => p.YTId == ytid).FirstOrDefaultAsync();
        return entity;
    }

    public async Task<IEnumerable<Video>> GetByChannel(string YTChannelID)
    {
        var entity = await _context.Videos
            .Where(p => p.IsDeleted == false)
            .Where(p => p.YTChannelId == YTChannelID)
            .ToListAsync();

        return entity;
    }

    public async Task<IEnumerable<Video>> GetByChannel(Guid ChannelID)
    {
        var entity = await _context.Videos
            .Where(p => p.IsDeleted == false)
            .Where(p => p.ChannelId == ChannelID)
            .ToListAsync();

        return entity;
    }

    public async Task<int> Update(List<Video> videos)
    {
        foreach (var video in videos)
        {
            var entity = await _context.Videos.FindAsync(video.Id);

            if (entity == null)
            {
                continue;
            }

            _context.Entry(entity).CurrentValues.SetValues(video);
        }

        await _context.SaveChangesAsync();

        return videos.Count();
    }

    public async Task<IEnumerable<Video>> GetByCategory(Guid categoryId)
    {
        var categoryWithChannels = await _context.Categories
            .Include(c => c.Channels)
                .ThenInclude(c => c.Videos)
            .FirstOrDefaultAsync(c => c.Id == categoryId);

        if (categoryWithChannels == null)
            return new List<Video>();

        return categoryWithChannels.Channels
            .SelectMany(c => c.Videos)
            .Where(v => !v.IsDeleted)
            .ToList();
    }
}
