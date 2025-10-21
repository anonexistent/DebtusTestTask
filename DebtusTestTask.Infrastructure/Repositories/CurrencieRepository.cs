using DebtusTestTask.Application.Repositories;
using DebtusTestTask.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace DebtusTestTask.Infrastructure.Repositories;

public class CurrencieRepository : ICurrencieRepository
{
    private readonly DebtusContext _context;

    public CurrencieRepository(DebtusContext context)
    {
        _context = context;
    }

    public async Task<Currency> FirsrOrDefaultAsync(Expression<Func<Currency, bool>> predicate)
    {
        return await _context.Currencies.FirstOrDefaultAsync(predicate);
    }

    public async Task<ICollection<Currency>> GetAllAsync()
    {
        return await _context.Currencies.ToListAsync();
    }
}
