namespace Basket.API.Basket.GetBasket;

public class GetBasketEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/basket/{userName}", async (string userName, ISender sender) =>
        {
            var result = await sender.Send(new GetBasketQuery(userName)).ConfigureAwait(false);

            var response = result.Adapt<GetBasketsResponse>();

            return Results.Ok(response);
        })
            .WithName("GetBasket")
            .Produces<GetBasketsResponse>(StatusCodes.Status200OK)
            .ProducesProblem(StatusCodes.Status404NotFound)
            .WithSummary("Get Basket")
            .WithDescription("Get Basket");
    }
}

public record GetBasketsResponse(ShoppingCart Cart);
