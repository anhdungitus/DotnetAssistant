using DotNetAssistant.Data;

namespace DotNetAssistant.Core.Caching;

public static class DnaEntityCacheDefaults<TEntity> where TEntity : BaseEntity
{
    public static string EntityTypeName => typeof(TEntity).Name.ToLowerInvariant();

    public static CacheKey ByIdCacheKey => new($"Dna.{EntityTypeName}.byid.{{0}}", ByIdPrefix, Prefix);

    public static CacheKey ByIdsCacheKey => new($"Dna.{EntityTypeName}.byids.{{0}}", ByIdsPrefix, Prefix);

    public static CacheKey AllCacheKey => new($"Dna.{EntityTypeName}.all.", AllPrefix, Prefix);

    public static string Prefix => $"Dna.{EntityTypeName}.";

    public static string ByIdPrefix => $"Dna.{EntityTypeName}.byid.";

    public static string ByIdsPrefix => $"Dna.{EntityTypeName}.byids.";

    public static string AllPrefix => $"Dna.{EntityTypeName}.all.";
}