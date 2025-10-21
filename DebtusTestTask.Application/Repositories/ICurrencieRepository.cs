using DebtusTestTask.Models;
using System.Linq.Expressions;

namespace DebtusTestTask.Application.Repositories;

public interface ICurrencieRepository
{
    Task<Currency> FirsrOrDefaultAsync(Expression<Func<Currency, bool>> predicate);
    Task<ICollection<Currency>> GetAllAsync();
}
