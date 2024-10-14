using Microsoft.AspNetCore.Http;

namespace Khuyaway.Presenters;

public abstract class SuccessPresenter<TResponse>
{
    public IResult Result { get; protected set; }

    protected virtual Task SuccessAsync(in TResponse? response, CancellationToken cancellationToken = default)
    {
        Result = Results.Ok(response);
        return Task.CompletedTask;
    }
}