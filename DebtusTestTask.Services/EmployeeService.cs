using DebtusTestTask.Application;
using DebtusTestTask.Contracts.Input;
using DebtusTestTask.Infrastructure;
using DebtusTestTask.Models;
using DebtusTestTask.Utils;
using Microsoft.EntityFrameworkCore;

namespace DebtusTestTask.Services;

public class EmployeeService : IEmployeeService
{
    private readonly DebtusContext _context;

    public EmployeeService(DebtusContext context)
    {
        _context = context;
    }

    public async Task<ServiceResult<Employee>> CreateEmployeeAsync(EmployeeCreateBody request)
    {
        var currs = await _context.Currencies.ToListAsync();

        var allEmps = await _context.Employees.ToListAsync();

        var isExistendId = await _context.Employees.AnyAsync(x=>x.Id == request.Id);

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

        _context.Employees.Add(employee);
        await _context.SaveChangesAsync();

        return new ServiceResult<Employee>()
        {
            IsSuccessfull = true,
            Result = employee,
        };
    }
}
