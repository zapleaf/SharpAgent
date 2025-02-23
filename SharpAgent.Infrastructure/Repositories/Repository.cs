using Microsoft.EntityFrameworkCore;
using SharpAgent.Application.IRepositories;
using SharpAgent.Infrastructure.Data;
using System.Linq.Expressions;

namespace SharpAgent.Infrastructure.Repositories;

public class Repository<T> : IRepository<T> where T : class
{
    private readonly AppDbContext _context;

    public Repository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<T> Create(T entity)
    {
        if (entity == null)
        {
            throw new ArgumentNullException(nameof(entity));
        }

        await _context.Set<T>().AddAsync(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task<List<T>> GetAll()
    {
        return await _context.Set<T>().ToListAsync();
    }

    /// <summary>
    /// Retrieves all entities of type T with optional related entities included.
    /// </summary>
    /// <param name="includeProperties">
    /// Variable number of expressions defining which related entities to include in the query.
    /// Each expression should specify a navigation property to include.
    /// </param>
    /// <returns>List of all entities with specified related entities included.</returns>
    /// <example>
    /// <code>
    /// // Get all channels with their videos and categories included
    /// var channels = await repository.GetAll(
    ///     c => c.Videos,    // Include Videos navigation property
    ///     c => c.Categories // Include Categories navigation property
    /// );
    /// 
    /// // Get all videos with just the channel included
    /// var videos = await repository.GetAll(v => v.Channel);
    /// </code>
    /// </example>
    public async Task<List<T>> GetAll(params Expression<Func<T, object>>[] includeProperties)
    {
        IQueryable<T> query = _context.Set<T>();

        foreach (var includeProperty in includeProperties)
        {
            query = query.Include(includeProperty);
        }

        return await query.ToListAsync();
    }

    public async Task<T> Get(Guid id)
    {
        IQueryable<T> query = _context.Set<T>();

        return await query.FirstOrDefaultAsync(e => EF.Property<Guid>(e, "Id") == id);
    }

    /// <summary>
    /// Retrieves a single entity by its ID with optional related entities included.
    /// </summary>
    /// <param name="id">The ID of the entity to retrieve.</param>
    /// <param name="includeProperties">
    /// Variable number of expressions defining which related entities to include in the query.
    /// Each expression should specify a navigation property to include.
    /// </param>
    /// <returns>The entity with specified related entities included, or null if not found.</returns>
    /// <example>
    /// <code>
    /// // Get a channel with ID 1 and include its videos and categories
    /// var channel = await repository.Get(1,
    ///     c => c.Videos,    // Include Videos navigation property
    ///     c => c.Categories // Include Categories navigation property
    /// );
    /// 
    /// // Get a video with ID 5 and include its channel
    /// var video = await repository.Get(5, v => v.Channel);
    /// </code>
    /// </example>
    public async Task<T> Get(Guid id, params Expression<Func<T, object>>[] includeProperties)
    {
        IQueryable<T> query = _context.Set<T>();

        foreach (var includeProperty in includeProperties)
        {
            query = query.Include(includeProperty);
        }

        return await query.FirstOrDefaultAsync(e => EF.Property<Guid>(e, "Id") == id);
    }

    public async Task<T> Update(T entity)
    {
        if (entity == null)
        {
            throw new ArgumentNullException(nameof(entity));
        }

        _context.Set<T>().Update(entity);
        await _context.SaveChangesAsync();
        return entity;
    }

    public async Task<bool> Delete(Guid id)
    {
        var entity = await _context.Set<T>().FindAsync(id);
        if (entity == null)
        {
            return false;
        }

        _context.Set<T>().Remove(entity);
        await _context.SaveChangesAsync();
        return true;
    }
}
