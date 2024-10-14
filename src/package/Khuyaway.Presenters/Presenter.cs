using FluentValidation.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Khuyaway.Presenters;

public abstract class Presenter<TResponse>(IOptions<ResultMessage> options)
{
    //TODO: Custom responses and logger (e.InnerException?.Message ?? e.Message)
    //TODO: Factory ??
    public IResult Result { get; protected set; }

    public virtual Task SuccessAsync(in TResponse? response, CancellationToken cancellationToken = default)
    {
        Result = Results.Ok(response);
        return Task.CompletedTask;
    }

    public virtual Task ValidationError(in IEnumerable<ValidationFailure> failures,
        CancellationToken cancellationToken = default)
    {
        Result = Results.ValidationProblem(
            failures.GroupBy(s => s.PropertyName, s => s.ErrorMessage).ToDictionary(g => g.Key, g => g.ToArray()),
            options.Value.ValidationError.Detail,
            $"{nameof(ProblemDetails)}/{nameof(ValidationProblemDetails)}",
            StatusCodes.Status400BadRequest,
            options.Value.ValidationError.Title,
            "https://datatracker.ietf.org/doc/html/rfc7231#section-6.5.1");
        return Task.CompletedTask;
    }

    public virtual Task ServerError(in Exception e, CancellationToken cancellationToken = default)
    {
        Result = Results.Problem(new ProblemDetails()
        {
            Status = StatusCodes.Status500InternalServerError,
            Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.6.1",
            Title = options.Value.ServerError.Title,
            Detail = options.Value.ServerError.Detail,
            Instance = $"{nameof(ProblemDetails)}/{e.GetType().Name}"
        });
        return Task.CompletedTask;
    }
}