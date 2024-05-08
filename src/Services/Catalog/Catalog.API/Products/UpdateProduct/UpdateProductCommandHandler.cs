namespace Catalog.API.Products.UpdateProduct;

internal class UpdateProductCommandHandler(
    IDocumentSession session,
    ILogger<UpdateProductCommandHandler> logger)
    : ICommandHandler<UpdateProductCommand, UpdateProductResult>
{
    private readonly IDocumentSession _session = session;
    private readonly ILogger<UpdateProductCommandHandler> _logger = logger;

    public async Task<UpdateProductResult> Handle(UpdateProductCommand command, CancellationToken cancellationToken)
    {
        _logger.LogInformation("UpdateProductCommandHandler.Handle called with {@Command}", command);

        var product = await _session.LoadAsync<Product>(command.Id, cancellationToken).ConfigureAwait(false)
            ?? throw new ProductNotFoundException();

        product.Name = command.Name;
        product.Description = command.Description;
        product.Categories = command.Categories;
        product.ImageFile = command.ImageFile;
        product.Price = command.Price;

        _session.Update(product);
        await _session.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

        return new UpdateProductResult(true);
    }
}


public record UpdateProductCommand(
    Guid Id,
    string Name,
    List<string> Categories,
    string Description,
    string ImageFile,
    decimal Price) : ICommand<UpdateProductResult>;

public record UpdateProductResult(bool IsSuccess);
