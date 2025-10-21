using DebtusTestTask.Application.Repositories;
using DebtusTestTask.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace DebtusTestTask.Infrastructure.Repositories;

public class EventRepository : IEventRepository
{
    private readonly DebtusContext _context;

    public EventRepository(DebtusContext context)
    {
        _context = context;
    }

    public async Task<Event> FirsrOrDefaultAsync(Expression<Func<Event, bool>> predicate)
    {
        return await _context.Events.FirstOrDefaultAsync(predicate);
    }

    public async Task<ICollection<Event>> GetAllAsync()
    {
        return await _context.Events.ToListAsync();
    }
}
