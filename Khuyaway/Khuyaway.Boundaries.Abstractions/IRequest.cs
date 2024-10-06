namespace Khuyaway.Boundaries.Abstractions;

public interface IRequest<out TRequest>
{
    TRequest Request { get; }
}