using FluentValidation.Results;
using Khuyaway.Common;
using Khuyaway.Presenters;
using Microsoft.AspNetCore.Http;

namespace Khuyaway.Abstractions;

public abstract class RequestHandler<TInput, TRequest, TResponse>(
    IValidateHandler<TInput, TRequest> validate,
    IHandlerPresenter<TResponse> presenter) : IHandler<TInput, TRequest>
    where TInput : class, IRequest<TRequest>
    where TRequest : class
{
    public async Task<IResult> HandleAsync(TInput input, CancellationToken cancellationToken = default)
    {
        try
        {
            var validatedAsync = await validate.SuccessfullyValidatedAsync(input, cancellationToken);
            if (!validatedAsync.resume) return validatedAsync.result;

            var response = await HandleUseCaseAsync(input, cancellationToken);

            var validatedAgainAsync = await validate.CheckAndSetErrorsAsync(cancellationToken);

            if (!validatedAgainAsync.resume) return validatedAgainAsync.result;

            return await presenter.SuccessAsync(response, cancellationToken);
        }
        catch (Exception e)
        {
            return await presenter.ServerErrorAsync(e, cancellationToken);
        }
    }

    protected void AddErrors(in List<ValidationFailure> errors) => validate.AddErrors(errors);

    protected abstract Task<TResponse> HandleUseCaseAsync(TInput input, CancellationToken cancellationToken = default);
}