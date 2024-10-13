namespace Khuyaway.Boundaries.Abstractions;

public abstract class ChainHandler<TInput>
{
    protected ChainHandler<TInput>? Successor;

    public ChainHandler<TInput> SetSuccessor(ChainHandler<TInput> successor)
        => Successor = successor;

    public abstract Task ProcessAsync(TInput input, CancellationToken cancellationToken = default);
}