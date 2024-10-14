using FluentValidation;
using FluentValidation.Results;
using Khuyaway.Common;
using Khuyaway.Presenters;
using Microsoft.AspNetCore.Http;

namespace Khuyaway.Abstractions;

internal class ValidateHandler<TInput, TRequest, TResponse>(
    IEnumerable<IValidator<TRequest>> validators,
    IHandlerPresenter<TResponse> presenter) : IValidateHandler<TInput, TRequest>
    where TInput : class, IRequest<TRequest>
    where TRequest : class
{
    private List<ValidationFailure> Errors { get; set; } = [];

    public void AddErrors(in List<ValidationFailure> errors) => Errors.AddRange(errors);

    public async Task<(bool resume, IResult? result)> SuccessfullyValidatedAsync(TInput input,
        CancellationToken cancellationToken = default)
    {
        if (!validators.Any()) return (true, default);

        var results = await Task.WhenAll(validators.Select(s => s.ValidateAsync(input.Request, cancellationToken)));

        Errors = results
            .Where(s => s.Errors.Count != 0)
            .SelectMany(s => s.Errors)
            .ToList();

        return await CheckAndSetErrorsAsync(cancellationToken);
    }

    public async Task<(bool resume, IResult? result)> CheckAndSetErrorsAsync(
        CancellationToken cancellationToken = default)
        => Errors.Count == 0
            ? (true, default)
            : (false, await presenter.ValidationErrorAsync(Errors, cancellationToken));
}