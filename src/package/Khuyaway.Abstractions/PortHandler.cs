using FluentValidation;
using FluentValidation.Results;
using Khuyaway.Boundaries;
using Khuyaway.Common;

namespace Khuyaway.Abstractions;

[Obsolete("Use RequestHandler or Handler instead.")]
public abstract class PortHandler<TInput, TRequest, TResponse>(
    IEnumerable<IValidator<TRequest>> validators,
    IOutputPort<TResponse> outputPort) : IInputPort<TInput>
    where TInput : class, IRequest<TRequest>
    where TRequest : class
{
    protected List<ValidationFailure> Errors { get; private set; } = [];

    public async Task HandleAsync(TInput input, CancellationToken cancellationToken = default)
    {
        try
        {
            if (validators.Any())
            {
                await ValidateAsync(input, cancellationToken);
                if (Errors.Count != 0)
                {
                    await outputPort.ValidationError(Errors);
                    return;
                }
            }

            var result = await RequestHandlerAsync(input, cancellationToken);

            if (Errors.Count != 0)
            {
                await outputPort.ValidationError(Errors);
                return;
            }

            await outputPort.SuccessAsync(result);
        }
        catch (Exception e)
        {
            await outputPort.ServerError(e);
        }
    }

    private async Task ValidateAsync(TInput input, CancellationToken cancellationToken = default)
    {
        var results = await Task.WhenAll(validators.Select(s => s.ValidateAsync(input.Request, cancellationToken)));

        Errors = results
            .Where(s => s.Errors.Count != 0)
            .SelectMany(s => s.Errors)
            .ToList();
    }

    protected abstract Task<TResponse> RequestHandlerAsync(TInput input, CancellationToken cancellationToken = default);
}