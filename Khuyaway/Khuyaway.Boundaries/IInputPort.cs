namespace Khuyaway.Boundaries;

public interface IInputPort<in TInput>
{
    Task HandleAsync(TInput input, CancellationToken cancellationToken = default);
}