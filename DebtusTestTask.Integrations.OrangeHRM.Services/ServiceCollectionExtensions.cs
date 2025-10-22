using DebtusTestTask.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace DebtusTestTask.Integrations.OrangeHRM.Services;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddOrangeServices(this IServiceCollection services)
    {
        services.AddTransient<IOrderService, OrderService>();
        services.AddTransient<IEmployeeService, EmployeeService>();

        return services;
    }
}
