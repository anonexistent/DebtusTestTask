using DebtusTestTask.Application.Repositories;
using DebtusTestTask.Models;
using System.Linq.Expressions;
using System.Text.Json;
using OrangeResult = DebtusTestTask.Integrations.OrangeHRM.Contracts.Output.SuccessEventGetResponse;

namespace DebtusTestTask.Integrations.OrangeHRM.Services.Repositories;

public class EventRepository : IEventRepository
{
    private readonly OrangeHttpClient _orangeHttpClient;

    public EventRepository(OrangeHttpClient orangeHttpClient)
    {
        _orangeHttpClient = orangeHttpClient;
    }

    public async Task<Event> FirsrOrDefaultAsync(Expression<Func<Event, bool>> predicate)
    {
        var (code, message) = await _orangeHttpClient.EventGetListAsync();

        var orangeResult = JsonSerializer.Deserialize<OrangeResult>(message)
            ?? throw new Exception("orange response parsing error");

        var result = orangeResult.Data.Select(x => new Event() { Id = x.Id, Name = x.Name }).FirstOrDefault(predicate.Compile());

        return result;
    }

    public async Task<ICollection<Event>> GetAllAsync()
    {
        var (code, message) = await _orangeHttpClient.EventGetListAsync();

        var orangeResult = JsonSerializer.Deserialize<OrangeResult>(message)
            ?? throw new Exception("orange response parsing error");

        var result = orangeResult.Data.Select(x => new Event() { Id = x.Id, Name = x.Name }).ToList();

        return result;
    }
}
