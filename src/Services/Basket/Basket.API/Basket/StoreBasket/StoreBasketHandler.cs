using Discount.Grpc;

namespace Basket.API.Basket.StoreBasket;

public class StoreBasketCommandHandler(
    IBasketRepository repository,
    DiscountProtoService.DiscountProtoServiceClient discountProto)
    : ICommandHandler<StoreBasketCommand, StoreBasketResult>
{
    private readonly IBasketRepository _repository = repository;
    private readonly DiscountProtoService.DiscountProtoServiceClient _discountProto = discountProto;

    public async Task<StoreBasketResult> Handle(StoreBasketCommand command, CancellationToken cancellationToken)
    {
        await ApplyDiscount(command.Cart, cancellationToken);

        await _repository.StoreBasket(
            command.Cart,
            cancellationToken).ConfigureAwait(false);

        return new(command.Cart.UserName);
    }

    private async Task ApplyDiscount(ShoppingCart cart, CancellationToken cancellationToken)
    {
        foreach (var item in cart.Items)
        {
            var dicountItem = await _discountProto.GetDiscountAsync(
                new() { ProductName = item.ProductName }, 
                cancellationToken: cancellationToken);
            item.Price = dicountItem.Amount;
        }
    }
}

public class StoreBasketCommandValidator : AbstractValidator<StoreBasketCommand>
{
    public StoreBasketCommandValidator()
    {
        RuleFor(x => x.Cart).NotNull().WithMessage("Cart can not be null");
        RuleFor(x => x.Cart.UserName).NotEmpty().WithMessage("UserName is required");
    }
}

public record StoreBasketCommand(
    ShoppingCart Cart) : ICommand<StoreBasketResult>;

public record StoreBasketResult(string UserName);
