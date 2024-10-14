using Khuyaway.Common;
using Microsoft.AspNetCore.Http;

namespace Khuyaway.Abstractions;

public interface IHandler<in TInput, TRequest>
    where TInput : class, IRequest<TRequest>
    where TRequest : class
{
    Task<IResult> HandleAsync(TInput input, CancellationToken cancellationToken = default);
}