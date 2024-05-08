namespace Catalog.API.Products.GetProductsByCategory;

internal class GetProductsByCategoryQueryHandler(
    IDocumentSession session)
    : IQueryHandler<GetProductByCategoryQuery, GetProductByCategorydResult>
{
    private readonly IDocumentSession _session = session;

    public async Task<GetProductByCategorydResult> Handle(GetProductByCategoryQuery query, CancellationToken cancellationToken)
    {
        var products = await _session.Query<Product>()
            .Where(x => x.Categories.Contains(query.Category))
            .ToListAsync(cancellationToken)
            .ConfigureAwait(false);

        return new(products);
    }
}

public record GetProductByCategoryQuery(string Category) : IQuery<GetProductByCategorydResult>;

public record GetProductByCategorydResult(IEnumerable<Product> Products);
