using DebtusTestTask.Integrations.OrangeHRM;
using DebtusTestTask.Models;
using System.Text.Json;
using OrangeResult = DebtusTestTask.Integrations.OrangeHRM.Contracts.Output.SuccessEmployeeGetResponse;

namespace DebtusTestTask.Infrastructure.Data;

public class EmployeeDataSeeder
{
    private readonly OrangeHttpClient _orangeHttpClient;
    private readonly DebtusContext _debtusContext;

    public EmployeeDataSeeder(OrangeHttpClient orangeHttpClient, DebtusContext debtusContext)
    {
        _orangeHttpClient = orangeHttpClient;
        _debtusContext = debtusContext;
    }

    public async Task<ICollection<Employee>> SeedAsync()
    {
        var (code, message) = await _orangeHttpClient.EmployeeGetListAsync();

        if(code is System.Net.HttpStatusCode.OK)
        {
            Console.WriteLine("try get emplist for seeding...");

            var orangeResult = JsonSerializer.Deserialize<OrangeResult>(message)
                ?? throw new Exception("orange response parsing error");

            var result = orangeResult.Data
                .Select(x => new Employee() { EmpNumber = x.EmpNumber, FirstName = x.FirstName, LastName = x.LastName, Id = x.EmpId, MiddleName = x.MiddleName, })
                .ToList();

            await _debtusContext.Employees.AddRangeAsync(result);

            return result;
        }

        return [];
    }
}
