namespace Catalog.API.Products.UpdateProduct;

internal class UpdateProductCommandHandler(
    IDocumentSession session)
    : ICommandHandler<UpdateProductCommand, UpdateProductResult>
{
    private readonly IDocumentSession _session = session;

    public async Task<UpdateProductResult> Handle(UpdateProductCommand command, CancellationToken cancellationToken)
    {
        var product = await _session.LoadAsync<Product>(command.Id, cancellationToken).ConfigureAwait(false)
            ?? throw new ProductNotFoundException(command.Id);

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

public class UpdateProductCommandValidator : AbstractValidator<UpdateProductCommand>
{
    public UpdateProductCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("Id is required");
        RuleFor(x => x.Name).NotEmpty().WithMessage("Name is required");
        RuleFor(x => x.Categories).NotEmpty().WithMessage("Categories is required");
        RuleFor(x => x.ImageFile).NotEmpty().WithMessage("ImageFile is required");
        RuleFor(x => x.Price).GreaterThan(0).WithMessage("Price must be greater than 0");
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
