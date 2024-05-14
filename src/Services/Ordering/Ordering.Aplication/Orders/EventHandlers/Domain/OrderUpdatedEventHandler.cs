namespace Ordering.Application.Orders.EventHandlers.Domain;

public class OrderUpdatedEventHandler(
    ILogger<OrderUpdatedEventHandler> logger)
    : INotificationHandler<OrderUpdatedEvent>
{
    private readonly ILogger<OrderUpdatedEventHandler> _logger = logger;

    public Task Handle(OrderUpdatedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Domain Event handled: {DomainEvent}", notification.GetType().Name);
        return Task.CompletedTask;
    }
}
