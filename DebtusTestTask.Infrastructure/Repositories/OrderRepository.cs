using DebtusTestTask.Application.Repositories;
using DebtusTestTask.Models;

namespace DebtusTestTask.Infrastructure.Repositories;

public class OrderRepository : IOrderRepository
{
    private readonly DebtusContext _context;

    public OrderRepository(DebtusContext context)
    {
        _context = context;
    }

    public async Task<Order> CreateAsync(Order o)
    {
        _context.Orders.Add(o);
        await _context.SaveChangesAsync();

        return o;
    }
}
