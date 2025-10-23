using AutoMapper;
using DebtusTestTask.Application.Repositories;
using DebtusTestTask.Application.Services;
using DebtusTestTask.Contracts.Input;
using DebtusTestTask.Models;
using DebtusTestTask.Utils;
using System.Text.Json;
using BodyDto = DebtusTestTask.Integrations.OrangeHRM.Contracts.Input.OrderCreateBody;
using OrangeResult = DebtusTestTask.Integrations.OrangeHRM.Contracts.Output.SuccessOrderResponse;

namespace DebtusTestTask.Integrations.OrangeHRM.Services;

public class OrderService : IOrderService
{
    private readonly IEmployeeRepository _employeeRepository;
    private readonly ICurrencieRepository _currencieRepository;
    private readonly IEventRepository _eventRepository;

    private readonly IOrderRepository _orderRepository;

    private readonly OrangeHttpClient _orangeHttpClient;

    private readonly IMapper _mapper;

    public OrderService(
        IEventRepository eventRepository,
        ICurrencieRepository currencieRepository,
        IEmployeeRepository employeeRepository,
        IOrderRepository orderRepository,
        OrangeHttpClient orangeHttpClient,
        IMapper mapper)
    {
        _eventRepository = eventRepository;
        _employeeRepository = employeeRepository;
        _currencieRepository = currencieRepository;
        _orderRepository = orderRepository;

        _orangeHttpClient = orangeHttpClient;

        _mapper = mapper;
    }

    public async Task<ServiceResult<Order>> CreateOrderAsync(OrderCreateBody body)
    {
        return await CreateOrderEntityAsync(body);
    }

    private async Task<ServiceResult<Order>> CreateOrderEntityAsync(OrderCreateBody body)
    {
        var b = _mapper.Map<BodyDto>(body);

        var (httpCode, message) = await _orangeHttpClient.OrderPostAsync(body.EmployeeNumber.ToString(), b);

        var orangeResult = JsonSerializer.Deserialize<OrangeResult>(message)
            ?? throw new Exception("orange response parsing error");

        if (httpCode is System.Net.HttpStatusCode.OK)
        {
            var employeeResult = await CommitOrderAsync(body);

            return employeeResult;
        }

        var orderResult = await CommitOrderAsync(body);

        return orderResult;
    }

    private async Task<ServiceResult<Order>> CommitOrderAsync(OrderCreateBody body)
    {
        var errors = new List<string>();

        var employee = await _employeeRepository.GetByNumberAsync(body.EmployeeNumber);
        var currency = await _currencieRepository.FirsrOrDefaultAsync(x => x.Name == body.CurrencyId);
        var @event = await _eventRepository.FirsrOrDefaultAsync(x => x.Id == body.ClaimEventId);

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
            EmployeeId = body.EmployeeNumber.ToString(),
            Currency = body.CurrencyId,
            Event = body.ClaimEventId.ToString(),
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
