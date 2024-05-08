using Marten.Pagination;

namespace Catalog.API.Products.GetProducts;

internal class GetProductsQueryHandler(
    IDocumentSession session)
    : IQueryHandler<GetProductsQuery, GetProductResult>
{
    private readonly IDocumentSession _session = session;

    public async Task<GetProductResult> Handle(GetProductsQuery query, CancellationToken cancellationToken)
    {
        var products = await _session.Query<Product>()
            .ToPagedListAsync(
                query.PageNumber, 
                query.PageSize, 
                cancellationToken)
            .ConfigureAwait(false);

        return new(products);
    }
}

public record GetProductsQuery(int PageNumber, int PageSize) : IQuery<GetProductResult>;

public record GetProductResult(IEnumerable<Product> Products);
