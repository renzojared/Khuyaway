using FluentValidation.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Khuyaway.Presenters;

public abstract class Presenter<TResponse>(IOptions<ResultMessage> options)
{
    //TODO: Custom responses and logger (e.InnerException?.Message ?? e.Message)
    //TODO: Factory ??

    protected virtual Task<IResult> SuccessAsync(in TResponse? response)
        => Task.FromResult(Results.Ok(response));

    protected virtual Task<IResult> ValidationError(in IEnumerable<ValidationFailure> failures)
        => Task.FromResult(Results.ValidationProblem(
            failures.GroupBy(s => s.PropertyName, s => s.ErrorMessage).ToDictionary(g => g.Key, g => g.ToArray()),
            options.Value.ValidationError.Detail,
            $"{nameof(ProblemDetails)}/{nameof(ValidationProblemDetails)}",
            StatusCodes.Status400BadRequest,
            options.Value.ValidationError.Title,
            "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.1"));

    protected virtual Task<IResult> ServerError(in Exception e)
        => Task.FromResult(Results.Problem(new ProblemDetails()
        {
            Status = StatusCodes.Status500InternalServerError,
            Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.6.1",
            Title = options.Value.ServerError.Title,
            Detail = options.Value.ServerError.Detail,
            Instance = $"{nameof(ProblemDetails)}/{e.GetType().Name}"
        }));
}