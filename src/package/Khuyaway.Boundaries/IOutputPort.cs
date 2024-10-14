namespace Khuyaway.Boundaries;

[Obsolete("Khuyaway.Boundaries.IOutputPort")]
public interface IOutputPort<TResponse>
{
    Task SuccessAsync(in TResponse? response, CancellationToken cancellationToken = default);
}