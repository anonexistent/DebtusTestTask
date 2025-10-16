using DebtusTestTask.Contracts.Input;
using DebtusTestTask.Models;
using DebtusTestTask.Utils;

namespace DebtusTestTask.Application;

public interface IOrderService
{
    Task<ServiceResult<Order>> CreateOrderAsync(OrderCreateBody orderCreateBody);
}
