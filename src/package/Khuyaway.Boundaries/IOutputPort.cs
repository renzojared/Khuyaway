namespace Khuyaway.Boundaries;

public interface IOutputPort<TResponse>
{
    Task SuccessAsync(in TResponse? response, CancellationToken cancellationToken = default);
}