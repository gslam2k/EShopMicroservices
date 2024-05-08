namespace Catalog.API.Products.GetProductById;

internal class GetProductByIdQueryHandler(
    IDocumentSession session)
    : IQueryHandler<GetProductByIdQuery, GetProductByIdResult>
{
    private readonly IDocumentSession _session = session;

    public async Task<GetProductByIdResult> Handle(GetProductByIdQuery query, CancellationToken cancellationToken)
    {
        var product = await _session.LoadAsync<Product>(query.Id, cancellationToken).ConfigureAwait(false);

        return product is not null
            ? new(product!)
            : throw new ProductNotFoundException(query.Id); ;
    }
}

public record GetProductByIdQuery(Guid Id) : IQuery<GetProductByIdResult>;

public record GetProductByIdResult(Product Product);
