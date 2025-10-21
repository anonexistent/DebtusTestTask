using DebtusTestTask.Models;

namespace DebtusTestTask.Application.Repositories;

public interface IOrderRepository
{
    public Task<Order> CreateAsync(Order o);
}
