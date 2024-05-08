namespace Catalog.API.Products.GetProductsByCategory;

internal class GetProductsByCategoryQueryHandler(
    IDocumentSession session,
    ILogger<GetProductsByCategoryQueryHandler> logger)
    : IQueryHandler<GetProductByCategoryQuery, GetProductByCategorydResult>
{
    private readonly IDocumentSession _session = session;
    private readonly ILogger<GetProductsByCategoryQueryHandler> _logger = logger;

    public async Task<GetProductByCategorydResult> Handle(GetProductByCategoryQuery query, CancellationToken cancellationToken)
    {
        _logger.LogInformation("GetProductByCategorydResult.Handle called with {query}", query);

        var products = await _session.Query<Product>()
            .Where(x => x.Categories.Contains(query.Category))
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false);

        return new(products);
    }
}

public record GetProductByCategoryQuery(string Category) : IQuery<GetProductByCategorydResult>;

public record GetProductByCategorydResult(IEnumerable<Product> Products);
