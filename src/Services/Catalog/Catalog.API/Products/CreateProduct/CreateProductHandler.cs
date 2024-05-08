namespace Catalog.API.Products.CreateProduct;

internal class CreateProductCommandHandler(
    IDocumentSession session)
    : ICommandHandler<CreateProductCommand, CreateProductResult>
{
    private readonly IDocumentSession _session = session;

    public async Task<CreateProductResult> Handle(
        CreateProductCommand command, 
        CancellationToken cancellationToken)
    {
        var product = new Product
        {
            Name = command.Name,
            Categories = command.Categories,
            Price = command.Price,
            Description = command.Description,
            ImageFile = command.ImageFile,
        };

        _session.Store(product);
        await _session.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

        return new(product.Id);
    }
}

public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
{
    public CreateProductCommandValidator()
    {
        RuleFor(x => x.Name).NotEmpty().WithMessage("Name is required");
        RuleFor(x => x.Categories).NotEmpty().WithMessage("Categories is required");
        RuleFor(x => x.ImageFile).NotEmpty().WithMessage("ImageFile is required");
        RuleFor(x => x.Price).GreaterThan(0).WithMessage("Price must be greater than 0");
    }
}

public record CreateProductCommand(
    string Name,
    List<string> Categories,
    string Description,
    string ImageFile,
    decimal Price) : ICommand<CreateProductResult>;

public record CreateProductResult(Guid Id);
