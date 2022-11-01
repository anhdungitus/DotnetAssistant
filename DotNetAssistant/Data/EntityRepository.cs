using DotNetAssistant.Core;
using DotNetAssistant.Core.Caching;
using DotNetAssistant.Core.Events;
using DotNetAssistant.Data.Extensions;
using Microsoft.EntityFrameworkCore;

namespace DotNetAssistant.Data;

public sealed class EntityRepository<TEntity> : IRepository<TEntity> where TEntity : BaseEntity
{
    private readonly IStaticCacheManager _staticCacheManager;
    private readonly IEventPublisher _eventPublisher;
    private readonly ApplicationDbContext _context;
    
    public EntityRepository(ApplicationDbContext context, IStaticCacheManager staticCacheManager, IEventPublisher eventPublisher)
    {
        _staticCacheManager = staticCacheManager;
        _eventPublisher = eventPublisher;
        _context = context;
        Table = context.Set<TEntity>();
        TableDbSet = context.Set<TEntity>();
    }

    private IQueryable<TEntity>? Table { get; }
    private DbSet<TEntity> TableDbSet { get; }

    public async Task<TEntity> GetByIdAsync(int? id, Func<IStaticCacheManager, CacheKey?>? getCacheKey = null, bool includeDeleted = true)
    {
        if (!id.HasValue || id == 0)
            return null;

        async Task<TEntity> GetEntityAsync()
        {
            return await Table.FirstOrDefaultAsync(entity => entity.Id == Convert.ToInt32(id));
        }
        
        if (getCacheKey == null)
            return await GetEntityAsync();

        //caching
        var cacheKey = getCacheKey(_staticCacheManager)
                       ?? _staticCacheManager.PrepareKeyForDefaultCache(DnaEntityCacheDefaults<TEntity>.ByIdCacheKey, id);

        return await _staticCacheManager.GetAsync(cacheKey, GetEntityAsync);
    }

    public async Task<IList<TEntity>> GetByIdsAsync(IList<int> ids, Func<IStaticCacheManager, CacheKey?>? getCacheKey = null, bool includeDeleted = true)
    {
        if (!ids?.Any() ?? true)
            return new List<TEntity>();

        async Task<IList<TEntity>> getByIdsAsync()
        {
            var query = Table;

            //get entries
            var entries = await query.Where(entry => ids.Contains(entry.Id)).ToListAsync();

            //sort by passed identifiers
            var sortedEntries = new List<TEntity>();
            foreach (var id in ids)
            {
                var sortedEntry = entries.Find(entry => entry.Id == id);
                if (sortedEntry != null)
                    sortedEntries.Add(sortedEntry);
            }

            return sortedEntries;
        }

        if (getCacheKey == null)
            return await getByIdsAsync();

        //caching
        var cacheKey = getCacheKey(_staticCacheManager)
                       ?? _staticCacheManager.PrepareKeyForDefaultCache(DnaEntityCacheDefaults<TEntity>.ByIdsCacheKey, ids);
        return await _staticCacheManager.GetAsync(cacheKey, getByIdsAsync);
    }

    public async Task<IList<TEntity>> GetAllAsync(Func<IQueryable<TEntity>, IQueryable<TEntity>> func = null, Func<IStaticCacheManager, CacheKey> getCacheKey = null, bool includeDeleted = true)
    {
        async Task<IList<TEntity>> getAllAsync()
        {
            var query = Table;
            query = func != null ? func(query) : query;

            return await query.ToListAsync();
        }

        return await GetEntitiesAsync(getAllAsync, getCacheKey);
    }

    public async Task<IList<TEntity>> GetAllAsync(Func<IQueryable<TEntity>, Task<IQueryable<TEntity>>> func = null, Func<IStaticCacheManager, CacheKey?>? getCacheKey = null, bool includeDeleted = true)
    {
        async Task<IList<TEntity>> getAllAsync()
        {
            var query = Table;
            query = func != null ? await func(query) : query;

            return await query.ToListAsync();
        }

        return await GetEntitiesAsync(getAllAsync, getCacheKey);
    }

    public IList<TEntity> GetAll(Func<IQueryable<TEntity>, IQueryable<TEntity>> func = null, Func<IStaticCacheManager, CacheKey?>? getCacheKey = null, bool includeDeleted = true)
    {
        IList<TEntity> getAll()
        {
            var query = Table;
            query = func != null ? func(query) : query;

            return query.ToList();
        }

        return GetEntities(getAll, getCacheKey);
    }

    public async Task<IList<TEntity>> GetAllAsync(Func<IQueryable<TEntity>, Task<IQueryable<TEntity>>> func, Func<IStaticCacheManager, Task<CacheKey?>>? getCacheKey, bool includeDeleted = true)
    {
        async Task<IList<TEntity>> getAllAsync()
        {
            var query = Table;
            query = func != null ? await func(query) : query;

            return await query.ToListAsync();
        }

        return await GetEntitiesAsync(getAllAsync, getCacheKey);
    }

    public async Task<IPagedList<TEntity>> GetAllPagedAsync(Func<IQueryable<TEntity>, IQueryable<TEntity>>? func = null, int pageIndex = 0, int pageSize = Int32.MaxValue,
        bool getOnlyTotalCount = false, bool includeDeleted = true)
    {
        var query = Table;

        query = func != null ? func(query) : query;

        return await query.ToPagedListAsync(pageIndex, pageSize, getOnlyTotalCount);
    }

    public async Task<IPagedList<TEntity>> GetAllPagedAsync(Func<IQueryable<TEntity>, Task<IQueryable<TEntity>>>? func = null,
        int pageIndex = 0, int pageSize = int.MaxValue, bool getOnlyTotalCount = false)
    {
        var query = Table;

        query = func != null ? await func(query) : query;

        return await query.ToPagedListAsync(pageIndex, pageSize, getOnlyTotalCount);
    }

    public async Task<TEntity> InsertAsync(TEntity entity)
    {
        var entityEntry = await TableDbSet.AddAsync(entity);
        await _context.SaveChangesAsync();
        await _eventPublisher.PublishAsync(new EntityInsertedEvent<TEntity>(entity));
        // await _staticCacheManager.RemoveByPrefixAsync(DnaEntityCacheDefaults<TEntity>.Prefix);
        return entityEntry.Entity;
    }

    public async Task<IList<TEntity>> InsertAsync(IList<TEntity> entities)
    {
        if (entities == null)
            throw new ArgumentNullException(nameof(entities));

        await TableDbSet.AddRangeAsync(entities);
        await _context.SaveChangesAsync();
        await _staticCacheManager.RemoveByPrefixAsync(DnaEntityCacheDefaults<TEntity>.Prefix);

        return entities;
    }

    public async Task<TEntity> UpdateAsync(TEntity entity)
    {
        var entry = TableDbSet.Update(entity);
        await _context.SaveChangesAsync();
        await _staticCacheManager.RemoveByPrefixAsync(DnaEntityCacheDefaults<TEntity>.ByIdPrefix);

        return entry.Entity;
    }

    public async Task UpdateAsync(IList<TEntity> entities)
    {
        TableDbSet.UpdateRange(entities);
        await _context.SaveChangesAsync();
        await _staticCacheManager.RemoveByPrefixAsync(DnaEntityCacheDefaults<TEntity>.ByIdPrefix);
    }

    public async Task DeleteAsync(TEntity entity)
    {
        TableDbSet.Remove(entity);
        await _context.SaveChangesAsync();
        await _staticCacheManager.RemoveByPrefixAsync(DnaEntityCacheDefaults<TEntity>.ByIdPrefix);
    }

    public async Task DeleteAsync(IList<TEntity> entities)
    {
        TableDbSet.RemoveRange(entities);
        await _context.SaveChangesAsync();
        await _staticCacheManager.RemoveByPrefixAsync(DnaEntityCacheDefaults<TEntity>.ByIdPrefix);
    }


    #region Utils
    protected async Task<IList<TEntity>> GetEntitiesAsync(Func<Task<IList<TEntity>>>? getAllAsync, Func<IStaticCacheManager, CacheKey?>? getCacheKey)
    {
        if (getCacheKey == null)
            return await getAllAsync();

        //caching
        var cacheKey = getCacheKey(_staticCacheManager)
                       ?? _staticCacheManager.PrepareKeyForDefaultCache(DnaEntityCacheDefaults<TEntity>.AllCacheKey);
        return await _staticCacheManager.GetAsync(cacheKey, getAllAsync);
    }
    
    protected async Task<IList<TEntity>> GetEntitiesAsync(Func<Task<IList<TEntity>>> getAllAsync, Func<IStaticCacheManager, Task<CacheKey?>>? getCacheKey)
    {
        if (getCacheKey == null)
            return await getAllAsync();

        //caching
        var cacheKey = await getCacheKey(_staticCacheManager)
                       ?? _staticCacheManager.PrepareKeyForDefaultCache(DnaEntityCacheDefaults<TEntity>.AllCacheKey);
        return await _staticCacheManager.GetAsync(cacheKey, getAllAsync);
    }
    
    protected IList<TEntity> GetEntities(Func<IList<TEntity>> getAll, Func<IStaticCacheManager, CacheKey?>? getCacheKey)
    {
        if (getCacheKey == null)
            return getAll();

        //caching
        var cacheKey = getCacheKey(_staticCacheManager)
                       ?? _staticCacheManager.PrepareKeyForDefaultCache(DnaEntityCacheDefaults<TEntity>.AllCacheKey);

        return _staticCacheManager.Get(cacheKey, getAll);
    }

    #endregion
}


public abstract partial class BaseEntity
{
    public int Id { get; set; }
}