using DebtusTestTask.Models;
using System.Linq.Expressions;

namespace DebtusTestTask.Application.Repositories;

public interface IEventRepository
{
    Task<Event> FirsrOrDefaultAsync(Expression<Func<Event, bool>> predicate);
    Task<ICollection<Event>> GetAllAsync();
}
