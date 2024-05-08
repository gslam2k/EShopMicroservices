namespace Catalog.API.Products.GetProducts;

public class GetProductsEndPoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/products", async ([AsParameters] GetProductsRequest request, ISender sender) =>
        {
            var query = request.Adapt<GetProductsQuery>();

            var result = await sender.Send(query).ConfigureAwait(false);

            var response = result.Adapt<GetProductsResponse>();

            return Results.Ok(response);
        })
            .WithName("GetProducts")
            .Produces<GetProductsResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithSummary("Get Products")
            .WithDescription("Get Products");
    }
}

public record GetProductsRequest(int? PageNumber = 1, int? PageSize = 10);

public record GetProductsResponse(List<Product> Products);
