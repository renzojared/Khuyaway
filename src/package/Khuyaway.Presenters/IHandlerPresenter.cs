using FluentValidation.Results;
using Microsoft.AspNetCore.Http;

namespace Khuyaway.Presenters;

public interface IHandlerPresenter<TResponse>
{
    Task<IResult> SuccessAsync(in TResponse? response, CancellationToken cancellationToken = default);

    Task<IResult> ValidationErrorAsync(in IEnumerable<ValidationFailure> failures,
        in CancellationToken cancellationToken = default);

    Task<IResult> ServerErrorAsync(in Exception exception, in CancellationToken cancellationToken = default);
}