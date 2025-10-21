using DebtusTestTask.Models;

namespace DebtusTestTask.Application.Repositories;

public interface IEmployeeRepository
{
    public Task<ICollection<Employee>> GetAllAsync();
    public Task<Employee> GetByIdAsync(string employeeId);
    public Task<bool> AnyByIdAsync(string employeeId);
    public Task<Employee> CreateAsync(Employee e);
}
