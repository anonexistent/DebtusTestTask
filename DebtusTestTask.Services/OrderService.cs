using DebtusTestTask.Application;
using DebtusTestTask.Contracts.Input;
using DebtusTestTask.Infrastructure;
using DebtusTestTask.Models;
using DebtusTestTask.Utils;
using Microsoft.EntityFrameworkCore;

namespace DebtusTestTask.Services;

public class OrderService : IOrderService
{
    private readonly DebtusContext _context;

    public OrderService(DebtusContext context)
    {
        _context = context;
    }

    public async Task<ServiceResult<Order>> CreateOrderAsync(OrderCreateBody body)
    {
        var errors = new List<string>();

        var employee = await _context.Employees.FirstOrDefaultAsync(x => x.Id == body.EmployeeId);
        var currency = await _context.Currencies.FirstOrDefaultAsync(x => x.Name == body.Currency);
        var @event = await _context.Events.FirstOrDefaultAsync(x => x.Name == body.Event);

        if (employee is null) 
            errors.Add("employee not found");
        if(currency is null) 
            errors.Add("currency not found");
        if(@event is null) 
            errors.Add("event not found");

        if (errors.Count > 0)
        {
            return new ServiceResult<Order>()
            {
                IsSuccessfull = false,
                Messages = errors,
            };
        }

        var order = new Order
        {
            EmployeeId = body.EmployeeId,
            Currency = body.Currency,
            Event = body.Event,
            Remarks = body.Remarks,

            Employee = employee,
        };

        _context.Orders.Add(order);
        await _context.SaveChangesAsync();

        return new ServiceResult<Order>() { IsSuccessfull = true, Result = order };
    }
}
