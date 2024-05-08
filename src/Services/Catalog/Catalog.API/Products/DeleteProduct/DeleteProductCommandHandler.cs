namespace Catalog.API.Products.DeleteProduct;

internal class DeleteProductCommandHandler(
    IDocumentSession session,
    ILogger<DeleteProductCommandHandler> logger)
    : ICommandHandler<DeleteProductCommand, DeleteProductResult>
{
    private readonly IDocumentSession _session = session;
    private readonly ILogger<DeleteProductCommandHandler> _logger = logger;

    public async Task<DeleteProductResult> Handle(DeleteProductCommand command, CancellationToken cancellationToken)
    {
        _logger.LogInformation("DeleteProductCommandHandler.Handle called with {@Command}", command);

        _session.Delete<Product>(command.Id);
        await _session.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

        return new DeleteProductResult(true);
    }
}


public record DeleteProductCommand(Guid Id) : ICommand<DeleteProductResult>;

public record DeleteProductResult(bool IsSuccess);
