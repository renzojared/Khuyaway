using Khuyaway.Common;
using Microsoft.Extensions.DependencyInjection;

namespace Khuyaway.Http;

public static class DependencyContainer
{
    public static IServiceCollection AddKhuyawayHttpServices(this IServiceCollection services)
    {
        services.AddScoped<ICurrentUser, CurrentUser>();

        return services;
    }
}