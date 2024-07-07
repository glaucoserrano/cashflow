using CashFlow.Domain.Entities;
using CashFlow.Domain.Repositories.Expenses;
using Microsoft.EntityFrameworkCore;

namespace CashFlow.Infrastructure.DataAcess.Repositories;
internal class ExpensesRepository : IExpensesReadOnlyRepository,IExpensesWriteOnlyRepository, IExpensesUpdateOnlyRepository
{
    private readonly CashFlowDbContext _dbcontext;
    public ExpensesRepository(CashFlowDbContext dbcontext)
    {
        _dbcontext = dbcontext;
    }
    public async Task Add(Expense expenses)
    {
        await _dbcontext.expenses.AddAsync(expenses);
        
    }

    public async Task<List<Expense>> GetAll()
    {
        return await _dbcontext.expenses.AsNoTracking().ToListAsync();
    }

    async Task<Expense?> IExpensesReadOnlyRepository.GetById(long id)
    {
        return await _dbcontext.expenses.AsNoTracking().FirstOrDefaultAsync(e => e.Id == id);
    }
    async Task<Expense?> IExpensesUpdateOnlyRepository.GetById(long id)
    {
        return await _dbcontext.expenses.FirstOrDefaultAsync(e => e.Id == id);
    }
    public async Task<bool> Delete(long id)
    {
        var result = await _dbcontext.expenses.FirstOrDefaultAsync(e => e.Id == id);

        if(result is null)
        {
            return false;
        }

        _dbcontext.expenses.Remove(result);

        return true;
    }

    public void Update(Expense expense)
    {
        _dbcontext.expenses.Update(expense);
    }

    public async Task<List<Expense>> FilterByMonth(DateOnly date)
    {
        
        var startDate = new DateTime(year: date.Year, month: date.Month, day: 1).Date;
        var daysInMonth = DateTime.DaysInMonth(year: date.Year, month: date.Month);
        var endDate = new DateTime(year: date.Year, month: date.Month, day: daysInMonth, hour: 23, minute: 59, second: 59);
        

        return await _dbcontext
            .expenses
            .AsNoTracking()
            .Where(e => e.Date >= startDate && e.Date <= endDate)
            .OrderBy(e => e.Date)
            .ThenBy(e => e.Title)
            .ToListAsync();
    }
}
