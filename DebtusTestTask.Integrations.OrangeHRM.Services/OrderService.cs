using DebtusTestTask.Application.Repositories;
using DebtusTestTask.Application.Services;
using DebtusTestTask.Contracts.Input;
using DebtusTestTask.Models;
using DebtusTestTask.Utils;

namespace DebtusTestTask.Integrations.OrangeHRM.Services;

public class OrderService : IOrderService
{
    private readonly IEmployeeRepository _employeeRepository;
    private readonly ICurrencieRepository _currencieRepository;
    private readonly IEventRepository _eventRepository;

    private readonly IOrderRepository _orderRepository;

    public OrderService(
        IEventRepository eventRepository,
        ICurrencieRepository currencieRepository,
        IEmployeeRepository employeeRepository,
        IOrderRepository orderRepository)
    {
        _eventRepository = eventRepository;
        _employeeRepository = employeeRepository;
        _currencieRepository = currencieRepository;
        _orderRepository = orderRepository;
    }

    public async Task<ServiceResult<Order>> CreateOrderAsync(OrderCreateBody body)
    {
        //  TODO: создание заказа через интеграцию

        var orderResult = await CommitOrderAsync(body);

        return orderResult;
    }

    private async Task<ServiceResult<Order>> CommitOrderAsync(OrderCreateBody body)
    {
        var errors = new List<string>();

        var employee = await _employeeRepository.GetByIdAsync(body.EmployeeId);
        var currency = await _currencieRepository.FirsrOrDefaultAsync(x => x.Name == body.Currency);
        var @event = await _eventRepository.FirsrOrDefaultAsync(x => x.Name == body.Event);

        if (employee is null)
            errors.Add("employee not found");
        if (currency is null)
            errors.Add("currency not found");
        if (@event is null)
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

        await _orderRepository.CreateAsync(order);

        return new ServiceResult<Order>()
        {
            IsSuccessfull = true,
            Result = order,
        };
    }
}
