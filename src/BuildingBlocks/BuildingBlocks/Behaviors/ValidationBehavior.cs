namespace BuildingBlocks.Behaviors;

public class ValidationBehavior<TRequest, TResponse>
    (IEnumerable<IValidator<TRequest>> validators)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : ICommand<TResponse>
{
    private readonly IValidator<TRequest>[] _validators = validators.ToArray();

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var context = new ValidationContext<TRequest>(request);
        var results =
            await Task.WhenAll(
                _validators.Select(v => v.ValidateAsync(context, cancellationToken)));

        var failures = results
            .Where(v => v.Errors.Count > 0)
            .SelectMany(r => r.Errors)
            .ToArray();

        if (failures.Length > 0) 
        {
            throw new ValidationException(failures);
        }

        return await next();
    }
}
