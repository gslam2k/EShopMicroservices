namespace Basket.API.Basket.GetBasket;

public class GetBasketQueryHandler(
    IBasketRepository repository)
    : IQueryHandler<GetBasketQuery, GetBasketResult>
{
    private readonly IBasketRepository _repository = repository;

    public async Task<GetBasketResult> Handle(GetBasketQuery request, CancellationToken cancellationToken)
    {
        var basket = await _repository.GetBasket(
            request.UserName,
            cancellationToken).ConfigureAwait(false);

        return new(basket);
    }
}

public record GetBasketQuery(string UserName) : IQuery<GetBasketResult>;

public record GetBasketResult(ShoppingCart Cart);