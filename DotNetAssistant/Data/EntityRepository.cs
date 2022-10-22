using DotNetAssistant.Core;
using DotNetAssistant.Data.Extensions;

namespace DotNetAssistant.Data;

public class EntityRepository<TEntity> : IRepository<TEntity> where TEntity : BaseEntity
{
    private ApplicationDbContext context;
    
    public EntityRepository(ApplicationDbContext context)
    {
        this.context = context;
        Table = context.Set<TEntity>();
    }

    protected virtual IQueryable<TEntity>? Table { get; set; }
    
    public virtual async Task<IPagedList<TEntity>> GetAllPagedAsync(Func<IQueryable<TEntity>, Task<IQueryable<TEntity>>>? func = null,
        int pageIndex = 0, int pageSize = int.MaxValue, bool getOnlyTotalCount = false)
    {
        var query = Table;

        query = func != null ? await func(query) : query;

        return await query.ToPagedListAsync(pageIndex, pageSize, getOnlyTotalCount);
    }
}


public abstract partial class BaseEntity
{
    /// <summary>
    /// Gets or sets the entity identifier
    /// </summary>
    public int Id { get; set; }
}