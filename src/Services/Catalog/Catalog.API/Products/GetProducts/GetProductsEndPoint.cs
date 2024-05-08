namespace Catalog.API.Products.GetProducts;

public class GetProductsEndPoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/products", async (ISender sender) =>
        {
            var query = new GetProductsQuery();

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

public record GetProductsRequest();

public record GetProductsResponse(List<Product> Products);
