namespace Catalog.API.Products.DeleteProduct;

internal class DeleteProductCommandHandler(
    IDocumentSession session)
    : ICommandHandler<DeleteProductCommand, DeleteProductResult>
{
    private readonly IDocumentSession _session = session;

    public async Task<DeleteProductResult> Handle(DeleteProductCommand command, CancellationToken cancellationToken)
    {
        _session.Delete<Product>(command.Id);
        await _session.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

        return new DeleteProductResult(true);
    }
}

public class DeleteProductCommandValidator : AbstractValidator<DeleteProductCommand>
{
    public DeleteProductCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("Id is required");
    }
}


public record DeleteProductCommand(Guid Id) : ICommand<DeleteProductResult>;

public record DeleteProductResult(bool IsSuccess);
