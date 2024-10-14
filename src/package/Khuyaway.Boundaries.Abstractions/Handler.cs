using FluentValidation;
using FluentValidation.Results;
using Khuyaway.Common;
using Khuyaway.Presenters;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace Khuyaway.Boundaries.Abstractions;

public abstract class Handler<TInput, TRequest, TResponse>(
    IEnumerable<IValidator<TRequest>> validators,
    IOptions<ResultMessage> options) : Presenter<TResponse>(options), IHandler<TInput, TRequest>
    where TInput : class, IRequest<TRequest>
    where TRequest : class
{
    protected List<ValidationFailure> Errors { get; set; } = [];

    public async Task<IResult> HandleAsync(TInput input, CancellationToken cancellationToken = default)
    {
        try
        {
            if (!await SuccessfullyValidatedAsync(input, cancellationToken)) return Result;

            var response = await HandleUseCaseAsync(input, cancellationToken);

            if (!await CheckAndSetErrorsAsync(cancellationToken)) return Result;

            await SuccessAsync(response, cancellationToken);
            return Result;
        }
        catch (Exception e)
        {
            await ServerError(e, cancellationToken);
            return Result;
        }
    }

    protected abstract Task<TResponse> HandleUseCaseAsync(TInput input, CancellationToken cancellationToken = default);

    private async Task<bool> SuccessfullyValidatedAsync(TInput input, CancellationToken cancellationToken = default)
    {
        if (!validators.Any()) return true;

        var results = await Task.WhenAll(validators.Select(s => s.ValidateAsync(input.Request, cancellationToken)));

        Errors = results
            .Where(s => s.Errors.Count != 0)
            .SelectMany(s => s.Errors)
            .ToList();

        return await CheckAndSetErrorsAsync(cancellationToken);
    }

    private async Task<bool> CheckAndSetErrorsAsync(CancellationToken cancellationToken = default)
    {
        if (Errors.Count == 0) return true;

        await ValidationError(Errors, cancellationToken);
        return false;
    }
}