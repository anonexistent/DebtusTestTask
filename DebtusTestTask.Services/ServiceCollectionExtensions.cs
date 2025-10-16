using DebtusTestTask.Application;
using Microsoft.Extensions.DependencyInjection;

namespace DebtusTestTask.Services;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddTransient<IOrderService, OrderService>();
        services.AddTransient<IEmployeeService, EmployeeService>();

        return services;
    }
}
