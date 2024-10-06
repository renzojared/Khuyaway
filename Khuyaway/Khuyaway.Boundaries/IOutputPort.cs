using FluentValidation.Results;

namespace Khuyaway.Boundaries;

public interface IOutputPort<TResponse>
{
    Task SuccessAsync(in TResponse? response);
    Task ValidationError(in IEnumerable<ValidationFailure> failures);
    Task ServerError(in Exception exception);
}