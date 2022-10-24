using DotNetAssistant.Core;
using DotNetAssistant.Data.Extensions;

namespace DotNetAssistant.Data;

public sealed class EntityRepository<TEntity> : IRepository<TEntity> where TEntity : BaseEntity
{
    private ApplicationDbContext _context;
    
    public EntityRepository(ApplicationDbContext context)
    {
        _context = context;
        Table = context.Set<TEntity>();
    }

    private IQueryable<TEntity>? Table { get; }
    
    public async Task<IPagedList<TEntity>> GetAllPagedAsync(Func<IQueryable<TEntity>, Task<IQueryable<TEntity>>>? func = null,
        int pageIndex = 0, int pageSize = int.MaxValue, bool getOnlyTotalCount = false)
    {
        var query = Table;

        query = func != null ? await func(query) : query;

        return await query.ToPagedListAsync(pageIndex, pageSize, getOnlyTotalCount);
    }
}


public abstract partial class BaseEntity
{
    public int Id { get; set; }
}