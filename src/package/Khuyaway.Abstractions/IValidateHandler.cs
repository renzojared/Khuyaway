using FluentValidation.Results;
using Khuyaway.Common;
using Microsoft.AspNetCore.Http;

namespace Khuyaway.Abstractions;

public interface IValidateHandler<in TInput, TRequest>
    where TInput : class, IRequest<TRequest>
    where TRequest : class
{
    void AddErrors(in List<ValidationFailure> errors);

    Task<(bool resume, IResult? result)> SuccessfullyValidatedAsync(TInput input,
        CancellationToken cancellationToken = default);

    Task<(bool resume, IResult? result)> CheckAndSetErrorsAsync(CancellationToken cancellationToken = default);
}