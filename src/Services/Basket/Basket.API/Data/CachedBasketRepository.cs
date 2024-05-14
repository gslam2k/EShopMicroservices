using System.Text.Json;

using Microsoft.Extensions.Caching.Distributed;

namespace Basket.API.Data;

public class CachedBasketRepository(
    IBasketRepository repository,
    IDistributedCache cache)
   : IBasketRepository
{
    private readonly IBasketRepository _repository = repository;
    private readonly IDistributedCache _cache = cache;

    public async Task<bool> DeleteBasket(
        string userName,
        CancellationToken cancellationToken = default)
    {
        await _repository.DeleteBasket(
            userName,
            cancellationToken).ConfigureAwait(false);

        await _cache.RemoveAsync(
            userName,
            cancellationToken).ConfigureAwait(false);

        return true;
    }

    public async Task<ShoppingCart> GetBasket(
        string userName,
        CancellationToken cancellationToken = default)
    {
        var cachedBasket = await _cache.GetStringAsync(
            userName,
            cancellationToken).ConfigureAwait(false);

        if (!string.IsNullOrWhiteSpace(cachedBasket))
        {
            return JsonSerializer.Deserialize<ShoppingCart>(cachedBasket)!;
        }

        var basket =  await _repository.GetBasket(
            userName,
            cancellationToken).ConfigureAwait(false);

        await _cache.SetStringAsync(
            userName,
            JsonSerializer.Serialize(basket),
            cancellationToken).ConfigureAwait(false);

        return basket;
    }

    public async Task<ShoppingCart> StoreBasket(
        ShoppingCart basket,
        CancellationToken cancellationToken = default)
    {
        await _repository.StoreBasket(
            basket,
            cancellationToken).ConfigureAwait(false);

        await _cache.SetStringAsync(
            basket.UserName,
            JsonSerializer.Serialize(basket),
            cancellationToken).ConfigureAwait(false);

        return basket;
    }
}
