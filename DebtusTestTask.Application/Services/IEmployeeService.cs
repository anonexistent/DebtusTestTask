using DebtusTestTask.Contracts.Input;
using DebtusTestTask.Models;
using DebtusTestTask.Utils;

namespace DebtusTestTask.Application.Services;

public interface IEmployeeService
{
    Task<ServiceResult<Employee>> CreateEmployeeAsync(EmployeeCreateBody request);
}
