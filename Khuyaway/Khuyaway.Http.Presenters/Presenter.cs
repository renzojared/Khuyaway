using Khuyaway.Boundaries;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace Khuyaway.Http.Presenters;

public abstract class Presenter<TResponse>(IOptions<HttpResultMessages> options)
    : ResultPresenter(options), IOutputPort<TResponse>
{
    public Task SuccessAsync(in TResponse? response)
    {
        Result = Results.Ok(response);
        return Task.CompletedTask;
    }
}