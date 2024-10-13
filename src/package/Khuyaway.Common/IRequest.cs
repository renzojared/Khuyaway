namespace Khuyaway.Common;

public interface IRequest<out TRequest>
{
    TRequest Request { get; }
}