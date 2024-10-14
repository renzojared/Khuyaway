using Microsoft.Extensions.DependencyInjection;

namespace Khuyaway.Presenters;

public static class DependencyContainer
{
    public static IServiceCollection AddKhuyawayPresenters(this IServiceCollection services,
        Action<ResultMessage> resultMessage)
    {
        services.Configure(resultMessage);
        services.AddScoped(typeof(IHandlerPresenter<>), typeof(HandlerPresenter<>));

        return services;
    }
}