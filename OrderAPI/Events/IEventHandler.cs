namespace OrderAPI.Events;

public interface IEventHandler<TEvent>
{
    Task HandleAsync(TEvent evt);
}
