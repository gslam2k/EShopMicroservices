namespace Catalog.API.Products.CreateProduct;

internal class CreateProductCommandHandler(IDocumentSession session)
    : ICommandHandler<CreateProductCommand, CreateProductResult>
{
    private readonly IDocumentSession _session = session;

    public async Task<CreateProductResult> Handle(
        CreateProductCommand request, 
        CancellationToken cancellationToken)
    {
        var product = new Product
        {
            Name = request.Name,
            Categories = request.Categories,
            Price = request.Price,
            Description = request.Description,
            ImageFile = request.ImageFile,
        };

        _session.Store(product);
        await _session.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

        return new(product.Id);
    }
}

public record CreateProductCommand(
    string Name,
    List<string> Categories,
    string Description,
    string ImageFile,
    decimal Price) : ICommand<CreateProductResult>;

public record CreateProductResult(Guid Id);
