using FluentValidation;
using FluentValidation.Results;
using Khuyaway.Common;
using Khuyaway.Presenters;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace Khuyaway.Abstractions;

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
            var validatedAsync = await SuccessfullyValidatedAsync(input, cancellationToken);
            if (!validatedAsync.resume) return validatedAsync.result;

            var response = await HandleUseCaseAsync(input, cancellationToken);

            var validatedAgainAsync = await CheckAndSetErrorsAsync();
            if (!validatedAgainAsync.resume) return validatedAgainAsync.result;

            return await SuccessAsync(response);
        }
        catch (Exception e)
        {
            return await ServerError(e);
        }
    }

    protected abstract Task<TResponse> HandleUseCaseAsync(TInput input, CancellationToken cancellationToken = default);

    private async Task<(bool resume, IResult? result)> SuccessfullyValidatedAsync(TInput input,
        CancellationToken cancellationToken = default)
    {
        if (!validators.Any()) return (true, default);

        var results = await Task.WhenAll(validators.Select(s => s.ValidateAsync(input.Request, cancellationToken)));

        Errors = results
            .Where(s => s.Errors.Count != 0)
            .SelectMany(s => s.Errors)
            .ToList();

        return await CheckAndSetErrorsAsync();
    }

    private async Task<(bool resume, IResult? result)> CheckAndSetErrorsAsync()
        => Errors.Count == 0 ? (true, default) : (false, await ValidationError(Errors));
}