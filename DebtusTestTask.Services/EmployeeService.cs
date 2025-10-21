using DebtusTestTask.Application.Repositories;
using DebtusTestTask.Application.Services;
using DebtusTestTask.Contracts.Input;
using DebtusTestTask.Models;
using DebtusTestTask.Utils;

namespace DebtusTestTask.Services;

public class EmployeeService : IEmployeeService
{
    private readonly IEmployeeRepository _employeeRepository;
    private readonly ICurrencieRepository _currencieRepository;

    public EmployeeService(
        ICurrencieRepository currencieRepository,
        IEmployeeRepository employeeRepository
        )
    {
        _employeeRepository = employeeRepository;
        _currencieRepository = currencieRepository;
    }

    public async Task<ServiceResult<Employee>> CreateEmployeeAsync(EmployeeCreateBody request)
    {
        var currs = await _currencieRepository.GetAllAsync();

        var allEmps = await _employeeRepository.GetAllAsync();

        var isExistendId = await _employeeRepository.AnyByIdAsync(request.Id);

        if(isExistendId)
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
