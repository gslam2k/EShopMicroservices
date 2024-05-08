namespace Catalog.API.Products.GetProductById;

internal class GetProductByIdQueryHandler(
    IDocumentSession session,
    ILogger<GetProductByIdQueryHandler> logger)
    : IQueryHandler<GetProductByIdQuery, GetProductByIdResult>
{
    private readonly IDocumentSession _session = session;
    private readonly ILogger<GetProductByIdQueryHandler> _logger = logger;

    public async Task<GetProductByIdResult> Handle(GetProductByIdQuery query, CancellationToken cancellationToken)
    {
        _logger.LogInformation("GetProductByIdQueryHandler.Handle called with {query}", query);
        var product = await _session.LoadAsync<Product>(query.Id, cancellationToken).ConfigureAwait(false);

        return product is not null
            ? new(product!)
            : throw new ProductNotFoundException(); ;
    }
}

public record GetProductByIdQuery(Guid Id) : IQuery<GetProductByIdResult>;

public record GetProductByIdResult(Product Product);
