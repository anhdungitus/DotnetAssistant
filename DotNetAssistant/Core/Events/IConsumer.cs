namespace DotNetAssistant.Core.Events;

public interface IConsumer<T>
{
    Task HandleEventAsync(T eventMessage);
}