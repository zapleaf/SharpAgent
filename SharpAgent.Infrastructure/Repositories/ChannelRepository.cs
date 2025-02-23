using SharpAgent.Infrastructure.Data;
using SharpAgent.Domain.Entities;

using Microsoft.EntityFrameworkCore;
using SharpAgent.Application.IRepositories;

namespace SharpAgent.Infrastructure.Repositories;

public class ChannelRepository : Repository<Channel>, IChannelRepository
{
    private readonly AppDbContext _context;

    public ChannelRepository(AppDbContext context) : base(context)
    {
        _context = context;
    }

    public async Task<bool> AddCategoryToChannel(Guid categoryId, Guid channelId)
    {
        // Retrieve the category and channel from the database
        var category = _context.Categories.SingleOrDefault(p => p.Id == categoryId);
        var channel = _context.Channels.Find(channelId);

        if (channel == null)
            throw new InvalidOperationException("Channel not found!");

        if (category == null)
            throw new InvalidOperationException("Category not found!");

        // Add the category to the channel's categories if not already added
        if (!channel.Categories.Contains(category))
        {
            channel.Categories.Add(category);
            // Could instead add channel to category -- category.Channels.Add(channel);
            await _context.SaveChangesAsync();

            return true;
        }

        return false;
    }

    public async Task<bool> RemoveCategoryFromChannel(Guid categoryId, Guid channelId)
    {
        // Retrieve the category and channel from the database
        var category = _context.Categories.SingleOrDefault(p => p.Id == categoryId);
        var channel = _context.Channels.Find(channelId);

        if (channel == null)
            throw new InvalidOperationException("Channel not found!");

        if (category == null)
            throw new InvalidOperationException("Category not found!");

        // Remove the category from the channel's categories if it exists
        if (channel.Categories.Contains(category))
        {
            channel.Categories.Remove(category);
            // Could instead add channel to category -- category.Channels.Add(channel);
            await _context.SaveChangesAsync();

            return true;
        }

        return false;
    }

    public async Task<IEnumerable<Channel>> GetWithCategories()
    {
        // Load channels with their categories
        return await GetAll(channel => channel.Categories);
    }

    public async Task<IEnumerable<Channel>> GetWithCategoriesAndVideos()
    {
        // Load channels with their categories and videos
        return await GetAll(channel => channel.Categories, channel => channel.Videos);
    }

    public async Task<IEnumerable<Channel>> GetByCategory(Guid categoryId)
    {
        try
        {
            // Find the Category and Include the Channels navigation property
            var categoryWithChannels = await _context.Categories
                                              .Include(p => p.Channels)
                                              .ThenInclude(c => c.Videos)
                                              .SingleOrDefaultAsync(p => p.Id == categoryId);

            // If the category exists, return the related channels
            return categoryWithChannels?.Channels.ToList() ?? new List<Channel>();
        }
        catch (Exception ex)
        {
            string message = ex.Message;
            string stackTrace = ex.StackTrace ?? "No Trace";
        }

        return null;
    }

    public new async Task<Channel> Get(Guid id)
    {
        var entity = await _context.Channels
            .Include(c => c.Videos)
            .Where(p => p.Id == id).FirstOrDefaultAsync();

        return entity;
    }

    public async Task<Channel> Get(string YTID)
    {
        var entity = await _context.Channels
            .Where(p => p.YTId == YTID).FirstOrDefaultAsync();

        return entity;
    }

    public new async Task<Channel> Update(Channel channel)
    {
        var entity = new Channel();

        if (channel != null)
            if (channel.Id != Guid.Empty)
                entity = await _context.Channels.FindAsync(channel.Id);
            else if (!string.IsNullOrEmpty(channel.YTId))
                entity = await _context.Channels.Where(p => p.YTId == channel.YTId).FirstOrDefaultAsync();

        if (entity == null)
            return null;

        _context.Entry(entity).CurrentValues.SetValues(channel);
        await _context.SaveChangesAsync();

        return entity;
    }

    public new async Task<bool> Delete(Guid idToDelete)
    {
        var entity = await _context.Channels
            .Include(c => c.Videos)
            .FirstOrDefaultAsync(c => c.Id == idToDelete);

        if (entity == null)
            return false;

        // Remove all related videos
        _context.Videos.RemoveRange(entity.Videos);

        // Remove the channel
        _context.Channels.Remove(entity);

        await _context.SaveChangesAsync();
        return true;
    }
}
