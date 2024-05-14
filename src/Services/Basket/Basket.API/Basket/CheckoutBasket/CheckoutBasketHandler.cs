namespace Basket.API.Basket.CheckoutBasket;

public record CheckoutBasketCommand(BasketCheckoutDto BasketCheckoutDto) 
    : ICommand<CheckoutBasketResult>;
public record CheckoutBasketResult(bool IsSuccess);

public class CheckoutBasketCommandValidator 
    : AbstractValidator<CheckoutBasketCommand>
{
    public CheckoutBasketCommandValidator()
    {
        RuleFor(x => x.BasketCheckoutDto).NotNull().WithMessage("BasketCheckoutDto can't be null");
        RuleFor(x => x.BasketCheckoutDto.UserName).NotEmpty().WithMessage("UserName is required");
    }
}

public class CheckoutBasketCommandHandler(
    IBasketRepository repository,
    IPublishEndpoint publishEndpoint)
    : ICommandHandler<CheckoutBasketCommand, CheckoutBasketResult>
{
    private readonly IBasketRepository _repository = repository;
    private readonly IPublishEndpoint _publishEndpoint = publishEndpoint;
    public async Task<CheckoutBasketResult> Handle(CheckoutBasketCommand command, CancellationToken cancellationToken)
    {
        // get existing basket with total price
        // Set totalprice on basketcheckout event message
        // send basket checkout event to rabbitmq using masstransit
        // delete the basket

        var basket = await _repository.GetBasket(
                command.BasketCheckoutDto.UserName,
                cancellationToken)
            .ConfigureAwait(false);

        if (basket == null)
        {
            return new CheckoutBasketResult(false);
        }

        var eventMessage = command.BasketCheckoutDto.Adapt<BasketCheckoutEvent>();
        eventMessage.TotalPrice = basket.TotalPrice;

        await _publishEndpoint.Publish(
            eventMessage,
            cancellationToken).ConfigureAwait(false);

        _ = await _repository.DeleteBasket(
            command.BasketCheckoutDto.UserName,
            cancellationToken).ConfigureAwait(false);

        return new CheckoutBasketResult(true);
    }
}
