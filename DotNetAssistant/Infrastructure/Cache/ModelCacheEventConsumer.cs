using DotNetAssistant.Core.Caching;
using DotNetAssistant.Core.Events;
using DotNetAssistant.Entities;

namespace DotNetAssistant.Infrastructure.Cache;

public class ModelCacheEventConsumer : IConsumer<EntityInsertedEvent<Question>>
{
    private readonly IStaticCacheManager _staticCacheManager;

    public ModelCacheEventConsumer(IStaticCacheManager staticCacheManager)
    {
        _staticCacheManager = staticCacheManager;
    }

    public async Task HandleEventAsync(EntityInsertedEvent<Question> eventMessage)
    {
        await _staticCacheManager.RemoveByPrefixAsync(DnaEntityCacheDefaults<Question>.Prefix);
    }
}