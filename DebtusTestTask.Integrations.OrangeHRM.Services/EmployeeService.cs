using AutoMapper;
using DebtusTestTask.Application.Repositories;
using DebtusTestTask.Application.Services;
using DebtusTestTask.Contracts.Input;
using DebtusTestTask.Models;
using DebtusTestTask.Utils;
using BodyDto = DebtusTestTask.Integrations.OrangeHRM.Contracts.Input.EmployeeCreateBody;

namespace DebtusTestTask.Integrations.OrangeHRM.Services;

public class EmployeeService : IEmployeeService
{
    private readonly IEmployeeRepository _employeeRepository;
    private readonly ICurrencieRepository _currencieRepository;

    private readonly OrangeHttpClient _orangeHttpClient;


    private readonly IMapper _mapper;

    public EmployeeService(
        ICurrencieRepository currencieRepository,
        IEmployeeRepository employeeRepository,
        OrangeHttpClient orangeHttpClient,
        IMapper mapper
        )
    {
        _employeeRepository = employeeRepository;
        _currencieRepository = currencieRepository;

        _orangeHttpClient = orangeHttpClient;

        _mapper = mapper;
    }

    public async Task<ServiceResult<Employee>> CreateEmployeeAsync(EmployeeCreateBody request)
    {
        var b = _mapper.Map<BodyDto>(request);

        var (httpCode, message) = await _orangeHttpClient.EmployeePostAsync(b);

        if(httpCode is System.Net.HttpStatusCode.OK)
        {
            var employeeResult = await CommitEmployeeAsync(request);

            return employeeResult;
        }

        return new ServiceResult<Employee>()
        {
            IsSuccessfull = false,
            Messages = [message]
        };
    }

    private async Task<ServiceResult<Employee>> CommitEmployeeAsync(EmployeeCreateBody request)
    {
        var currs = await _currencieRepository.GetAllAsync();

        var allEmps = await _employeeRepository.GetAllAsync();

        var isExistendId = await _employeeRepository.AnyByIdAsync(request.Id);

        if (isExistendId)
        {
            return new ServiceResult<Employee>()
            {
                IsSuccessfull = false,
                Messages = ["incorrect id"]
            };
        }

        var employee = new Employee
        {
            Id = request.Id,

            LastName = request.LastName,
            FirstName = request.FirstName,
            MiddleName = request.MiddleName,
        };

        await _employeeRepository.CreateAsync(employee);

        return new ServiceResult<Employee>()
        {
            IsSuccessfull = true,
            Result = employee,
        };
    }
}
