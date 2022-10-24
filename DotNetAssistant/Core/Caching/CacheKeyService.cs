using System.Globalization;
using System.Text;
using DotNetAssistant.Data;

namespace DotNetAssistant.Core.Caching;

public abstract class CacheKeyService
{
    private string HashAlgorithm => "SHA1";

    protected virtual string PrepareKeyPrefix(string prefix, params object[] prefixParameters)
    {
        return prefixParameters?.Any() ?? false
            ? string.Format(prefix, prefixParameters.Select(CreateCacheKeyParameters).ToArray())
            : prefix;
    }

    protected virtual string CreateIdsHash(IEnumerable<int> ids)
    {
        var identifiers = ids.ToList();

        if (!identifiers.Any())
            return string.Empty;
    
        var identifiersString = string.Join(", ", identifiers.OrderBy(id => id));
        return HashHelper.CreateHash(Encoding.UTF8.GetBytes(identifiersString), HashAlgorithm);
    }

    protected virtual object CreateCacheKeyParameters(object parameter)
    {
        return parameter switch
        {
            null => "null",
            IEnumerable<int> ids => CreateIdsHash(ids),
            IEnumerable<BaseEntity> entities => CreateIdsHash(entities.Select(entity => entity.Id)),
            BaseEntity entity => entity.Id,
            decimal param => param.ToString(CultureInfo.InvariantCulture),
            _ => parameter
        };
    }

    public virtual CacheKey PrepareKey(CacheKey cacheKey, params object[] cacheKeyParameters)
    {
        return cacheKey.Create(CreateCacheKeyParameters, cacheKeyParameters);
    }

    public virtual CacheKey PrepareKeyForDefaultCache(CacheKey cacheKey, params object[] cacheKeyParameters)
    {
        var key = cacheKey.Create(CreateCacheKeyParameters, cacheKeyParameters);

        key.CacheTime = CacheConfig.DefaultCacheTime;

        return key;
    }

    public virtual CacheKey PrepareKeyForShortTermCache(CacheKey cacheKey, params object[] cacheKeyParameters)
    {
        var key = cacheKey.Create(CreateCacheKeyParameters, cacheKeyParameters);

        key.CacheTime = CacheConfig.ShortTermCacheTime;

        return key;
    }
}

public static class CacheConfig
{
    /// <summary>
    /// Gets or sets the default cache time in minutes
    /// </summary>
    public static int DefaultCacheTime { get; private set; } = 60;

    /// <summary>
    /// Gets or sets the short term cache time in minutes
    /// </summary>
    public static int ShortTermCacheTime { get; private set; } = 3;

    /// <summary>
    /// Gets or sets the bundled files cache time in minutes
    /// </summary>
    public static int BundledFilesCacheTime { get; private set; } = 120;
}