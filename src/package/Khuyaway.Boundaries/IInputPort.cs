namespace Khuyaway.Boundaries;

[Obsolete("Khuyaway.Boundaries.IInputPort")]
public interface IInputPort<in TInput>
{
    Task HandleAsync(TInput input, CancellationToken cancellationToken = default);
}