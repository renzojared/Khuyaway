namespace Khuyaway.Boundaries.Abstractions;

public abstract class Handler<TInput>
{
    protected Handler<TInput>? Successor;

    public Handler<TInput> SetSuccessor(Handler<TInput> successor)
        => Successor = successor;

    public abstract Task ProcessAsync(TInput input, CancellationToken cancellationToken = default);
}