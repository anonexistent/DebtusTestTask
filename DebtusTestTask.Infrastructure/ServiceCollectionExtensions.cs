using DebtusTestTask.Application.Repositories;
using DebtusTestTask.Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;

namespace DebtusTestTask.Infrastructure;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddRepositories(this IServiceCollection services)
    {
        services.AddTransient<IEventRepository, EventRepository>();
        services.AddTransient<ICurrencieRepository, CurrencieRepository>();
        services.AddTransient<IOrderRepository, OrderRepository>();
        services.AddTransient<IEmployeeRepository, EmployeeRepository>();

        return services;
    }
}
