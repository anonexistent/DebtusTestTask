using Microsoft.Extensions.DependencyInjection;

namespace DebtusTestTask.Integrations.OrangeHRM;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddOrangeHRM(this IServiceCollection services)
    {
        services.AddTransient<OrangeHttpClient>();

        return services;
    }
}
