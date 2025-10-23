using DebtusTestTask.Integrations.OrangeHRM;
using DebtusTestTask.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using OrangeResult = DebtusTestTask.Integrations.OrangeHRM.Contracts.Output.SuccessEmployeeGetResponse;

namespace DebtusTestTask.Infrastructure.Data;

public class EmployeeDataSeeder
{
    private readonly OrangeHttpClient _orangeHttpClient;
    private readonly DebtusContext _debtusContext;

    private readonly ILogger<EmployeeDataSeeder> _logger;

    public EmployeeDataSeeder(
        OrangeHttpClient orangeHttpClient,
        DebtusContext debtusContext,
        ILogger<EmployeeDataSeeder> logger
        )
    {
        _orangeHttpClient = orangeHttpClient;
        _debtusContext = debtusContext;

        _logger = logger;
    }

    public async Task<ICollection<Employee>> SeedAsync()
    {
        var (code, message) = await _orangeHttpClient.EmployeeGetListAsync();

        if(code is System.Net.HttpStatusCode.OK)
        {
            try
            {
                _logger.LogInformation("try seeding...");

                var orangeResult = JsonSerializer.Deserialize<OrangeResult>(message)
                    ?? throw new Exception("orange response parsing error");

                var result = orangeResult.Data
                    .Where(x=>x.EmpId is not null)
                    .Select(x => new Employee() { EmpNumber = x.EmpNumber, FirstName = x.FirstName, LastName = x.LastName, Id = x.EmpId, MiddleName = x.MiddleName, })
                    .ToList();

                await _debtusContext.Employees.AddRangeAsync(result);
                await _debtusContext.SaveChangesAsync();

                var currentEmplList = await _debtusContext.Employees.ToListAsync();

                _logger.LogInformation("seeeding ok!");
                _logger.LogInformation("current user id (empNumber) list: [{userIdListString}]",
                    string.Join(",\t", currentEmplList.Select(x => x.Id + $" ({x.EmpNumber})").ToList()));

                return result;
            }
            catch (Exception ex) { }
        }

        return [];
    }
}
