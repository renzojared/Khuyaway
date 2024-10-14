using FluentValidation.Results;
using Microsoft.AspNetCore.Http;

namespace Khuyaway.Presenters;

public interface IHandlerPresenter
{
    public IResult Result { get; }

    void SetResult(in IResult result);

    Task ValidationErrorAsync(in IEnumerable<ValidationFailure> failures,
        in CancellationToken cancellationToken = default);

    Task ServerErrorAsync(in Exception exception, in CancellationToken cancellationToken = default);
}