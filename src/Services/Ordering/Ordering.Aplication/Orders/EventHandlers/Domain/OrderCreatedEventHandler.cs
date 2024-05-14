namespace Ordering.Application.Orders.EventHandlers.Domain;

public class OrderCreatedEventHandler(
 //   IPublishEndpoint publishEndpoint,
 //   IFeatureManager featureManager,
    ILogger<OrderCreatedEventHandler> logger)
    : INotificationHandler<OrderCreatedEvent>
{
    private readonly ILogger<OrderCreatedEventHandler> _logger = logger;
    public  Task Handle(OrderCreatedEvent domainEvent, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Domain Event handled: {DomainEvent}", domainEvent.GetType().Name);
/*
        if (await featureManager.IsEnabledAsync("OrderFullfilment"))
        {
            var orderCreatedIntegrationEvent = domainEvent.Order.ToOrderDto();
            await publishEndpoint.Publish(orderCreatedIntegrationEvent, cancellationToken);
        }
*/
        return Task.CompletedTask;
    }
}
