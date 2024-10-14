using Khuyaway.Presenters;
using Microsoft.Extensions.DependencyInjection;

namespace Khuyaway.Abstractions;

public static class DependencyContainer
{
    public static IServiceCollection AddKhuyawayAbstractions(this IServiceCollection services,
        Action<ResultMessage> resultMessage)
    {
        services.AddKhuyawayPresenters(resultMessage);
        services.AddScoped(typeof(IValidateHandler<,>), typeof(ValidateHandler<,,>));

        return services;
    }
}