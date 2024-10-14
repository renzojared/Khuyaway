using FluentValidation.Results;

namespace Khuyaway.Boundaries;

public interface IOutputPort<TResponse>
{
    Task SuccessAsync(in TResponse? response, CancellationToken cancellationToken = default);
    Task ValidationError(in IEnumerable<ValidationFailure> failures, CancellationToken cancellationToken = default);
    Task ServerError(in Exception exception, CancellationToken cancellationToken = default);
}