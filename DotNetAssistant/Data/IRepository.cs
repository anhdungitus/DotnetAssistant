using System.Linq.Expressions;
using DotNetAssistant.Core;
using DotNetAssistant.Core.Caching;

namespace DotNetAssistant.Data;

public interface IRepository<TEntity>
{
    Task<TEntity> GetByIdAsync(int? id, Func<IStaticCacheManager, CacheKey?>? getCacheKey = null, bool includeDeleted = true);
    Task<IList<TEntity>> GetByIdsAsync(IList<int> ids, Func<IStaticCacheManager, CacheKey?>? getCacheKey = null, bool includeDeleted = true);
    Task<IList<TEntity>> GetAllAsync(Func<IQueryable<TEntity>, IQueryable<TEntity>>? func = null,
        Func<IStaticCacheManager, CacheKey?>? getCacheKey = null, bool includeDeleted = true);
    Task<IList<TEntity>> GetAllAsync(Func<IQueryable<TEntity>, Task<IQueryable<TEntity>>>? func = null,
        Func<IStaticCacheManager, CacheKey?>? getCacheKey = null, bool includeDeleted = true);
    IList<TEntity> GetAll(Func<IQueryable<TEntity>, IQueryable<TEntity>>? func = null,
        Func<IStaticCacheManager, CacheKey?>? getCacheKey = null, bool includeDeleted = true);
    Task<IList<TEntity>> GetAllAsync(Func<IQueryable<TEntity>, Task<IQueryable<TEntity>>> func,
        Func<IStaticCacheManager, Task<CacheKey?>>? getCacheKey, bool includeDeleted = true);
    Task<IPagedList<TEntity>> GetAllPagedAsync(Func<IQueryable<TEntity>, IQueryable<TEntity>>? func = null,
        int pageIndex = 0, int pageSize = int.MaxValue, bool getOnlyTotalCount = false, bool includeDeleted = true);
    Task<IPagedList<TEntity>> GetAllPagedAsync(Func<IQueryable<TEntity>, Task<IQueryable<TEntity>>>? func = null,
        int pageIndex = 0, int pageSize = int.MaxValue, bool getOnlyTotalCount = false);
    Task<TEntity> InsertAsync(TEntity entity);
    Task<IList<TEntity>> InsertAsync(IList<TEntity> entities);
    Task<TEntity> UpdateAsync(TEntity entity);
    Task UpdateAsync(IList<TEntity> entities);
    Task DeleteAsync(TEntity entity);
    Task DeleteAsync(IList<TEntity> entities);
}