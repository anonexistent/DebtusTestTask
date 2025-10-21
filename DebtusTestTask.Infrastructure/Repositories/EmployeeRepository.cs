using DebtusTestTask.Application.Repositories;
using DebtusTestTask.Models;
using Microsoft.EntityFrameworkCore;

namespace DebtusTestTask.Infrastructure.Repositories;

public class EmployeeRepository : IEmployeeRepository
{
    private readonly DebtusContext _context;

    public EmployeeRepository(DebtusContext context)
    {
        _context = context;
    }

    public async Task<bool> AnyByIdAsync(string employeeId)
    {
        return await _context.Employees.AnyAsync(x => x.Id == employeeId);
    }

    public async Task<Employee> CreateAsync(Employee e)
    {
        await _context.AddAsync(e);
        await _context.SaveChangesAsync();

        return e;
    }

    public async Task<ICollection<Employee>> GetAllAsync()
    {
        return await _context.Employees.ToListAsync();
    }

    public async Task<Employee> GetByIdAsync(string employeeId)
    {
        return await _context.Employees.FirstOrDefaultAsync(x => x.Id == employeeId);
    }
}
