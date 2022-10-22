using DotNetAssistant.Core;

namespace DotNetAssistant.Data;

public interface IRepository<TEntity>
{
    Task<IPagedList<TEntity>> GetAllPagedAsync(Func<IQueryable<TEntity>, Task<IQueryable<TEntity>>> func = null,
        int pageIndex = 0, int pageSize = int.MaxValue, bool getOnlyTotalCount = false);
}