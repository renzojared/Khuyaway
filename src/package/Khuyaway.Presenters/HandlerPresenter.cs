using FluentValidation.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Khuyaway.Presenters;

public class HandlerPresenter<TResponse>(IOptions<ResultMessage> options) : IHandlerPresenter<TResponse>
{
    public Task<IResult> SuccessAsync(in TResponse? response, CancellationToken cancellationToken = default)
        => Task.FromResult(Results.Ok(response));

    public Task<IResult> ValidationErrorAsync(in IEnumerable<ValidationFailure> failures,
        in CancellationToken cancellationToken = default)
        => Task.FromResult(Results.ValidationProblem(
            failures.GroupBy(s => s.PropertyName, s => s.ErrorMessage).ToDictionary(g => g.Key, g => g.ToArray()),
            options.Value.ValidationError.Detail,
            $"{nameof(ProblemDetails)}/{nameof(ValidationProblemDetails)}",
            StatusCodes.Status400BadRequest,
            options.Value.ValidationError.Title,
            "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.1"));

    public Task<IResult> ServerErrorAsync(in Exception e, in CancellationToken cancellationToken = default)
        => Task.FromResult(Results.Problem(new ProblemDetails
        {
            Status = StatusCodes.Status500InternalServerError,
            Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.6.1",
            Title = options.Value.ServerError.Title,
            Detail = options.Value.ServerError.Detail,
            Instance = $"{nameof(ProblemDetails)}/{e.GetType().Name}"
        }));
}