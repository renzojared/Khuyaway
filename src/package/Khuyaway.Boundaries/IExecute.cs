namespace Khuyaway.Boundaries;

public interface IExecute
{
    Task ExecuteAsync(CancellationToken cancellationToken = default);
}