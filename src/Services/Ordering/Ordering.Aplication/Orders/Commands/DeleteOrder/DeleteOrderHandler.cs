namespace Ordering.Application.Orders.Commands.DeleteOrder;

public class DeleteOrderHandler(
    IApplicationDbContext dbContext)
    : ICommandHandler<DeleteOrderCommand, DeleteOrderResult>
{
    private readonly IApplicationDbContext _dbContext = dbContext;

    public async Task<DeleteOrderResult> Handle(
        DeleteOrderCommand command,
        CancellationToken cancellationToken)
    {
        //Delete Order entity from command object
        //save to database
        //return result

        var orderId = OrderId.Of(command.OrderId);
        var order = await _dbContext.Orders
            .FindAsync([orderId], cancellationToken: cancellationToken);

        if (order is null)
        {
            throw new OrderNotFoundException(command.OrderId);
        }

        _dbContext.Orders.Remove(order);
        await _dbContext.SaveChangesAsync(cancellationToken);

        return new DeleteOrderResult(true);        
    }
}
