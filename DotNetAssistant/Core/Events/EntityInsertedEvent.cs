using DotNetAssistant.Data;

namespace DotNetAssistant.Core.Events;

public partial class EntityInsertedEvent<T> where T : BaseEntity
{
    public EntityInsertedEvent(T entity)
    {
        Entity = entity;
    }
    public T Entity { get; }
}