using FluentValidation.Results;
using Microsoft.AspNetCore.Http;

namespace Khuyaway.Presenters;

public interface IHandlerPresenter<TResponse>
{
    public IResult Result { get; }

    Task SuccessAsync(in TResponse? response, CancellationToken cancellationToken = default);

    Task ValidationErrorAsync(in IEnumerable<ValidationFailure> failures,
        in CancellationToken cancellationToken = default);

    Task ServerErrorAsync(in Exception exception, in CancellationToken cancellationToken = default);
}