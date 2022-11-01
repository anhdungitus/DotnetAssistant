namespace DotNetAssistant.Core.Events;

public interface IEventPublisher
{
    Task PublishAsync<TEvent>(TEvent @event);
}

public class EventPublisher : IEventPublisher
{
    private readonly IServiceProvider  _serviceProvider;

    public EventPublisher(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task PublishAsync<TEvent>(TEvent @event)
    {
        var consumers = (IEnumerable<IConsumer<TEvent>>)_serviceProvider.GetServices(typeof(IConsumer<TEvent>));
        foreach (var consumer in consumers)
        {
            try
            {
                //try to handle published event
                await consumer.HandleEventAsync(@event);
            }
            catch (Exception exception)
            {
                //log error, we put in to nested try-catch to prevent possible cyclic (if some error occurs)
                await File.AppendAllLinesAsync("dotnetAssistant.txt", new []{exception.Message});
            }
        }
    }
}