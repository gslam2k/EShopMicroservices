namespace Catalog.API.Products.GetProducts;

internal class GetProductsQueryHandler(
    IDocumentSession session,
    ILogger<GetProductsQueryHandler> logger)
    : IQueryHandler<GetProductsQuery, GetProductResult>
{
    private readonly IDocumentSession _session = session;
    private readonly ILogger<GetProductsQueryHandler> _logger = logger;

    public async Task<GetProductResult> Handle(GetProductsQuery query, CancellationToken cancellationToken)
    {
        _logger.LogInformation("GetProductsQueryHandler.Handle called with {query}", query);

        var products = await _session.Query<Product>().ToListAsync(cancellationToken).ConfigureAwait(false);

        return new(products);
    }
}

public record GetProductsQuery() : IQuery<GetProductResult>;

public record GetProductResult(IEnumerable<Product> Products);
