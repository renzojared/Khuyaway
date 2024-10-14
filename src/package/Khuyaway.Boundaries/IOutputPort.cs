using FluentValidation.Results;

namespace Khuyaway.Boundaries;

[Obsolete("Khuyaway.Boundaries.IOutputPort")]
public interface IOutputPort<TResponse>
{
    Task SuccessAsync(in TResponse? response, CancellationToken cancellationToken = default);
    Task ValidationError(in IEnumerable<ValidationFailure> failures);
    Task ServerError(in Exception exception);
}