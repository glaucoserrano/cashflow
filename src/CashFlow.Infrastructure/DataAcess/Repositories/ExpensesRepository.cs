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

    public async Task<List<Expense>> GetAll(User user)
    {
        return await _dbcontext.expenses.AsNoTracking().Where(e => e.UserId == user.Id).ToListAsync();
    }

    async Task<Expense?> IExpensesReadOnlyRepository.GetById(User user ,long id)
    {
        return await _dbcontext.expenses.AsNoTracking().FirstOrDefaultAsync(e => e.Id == id && e.UserId == user.Id);
    }
    async Task<Expense?> IExpensesUpdateOnlyRepository.GetById(User user ,long id)
    {
        return await _dbcontext.expenses.FirstOrDefaultAsync(e => e.Id == id && e.UserId == user.Id);
    }
    public async Task Delete(long id)
    {
        var result = await _dbcontext.expenses.FirstAsync(e => e.Id == id);

        _dbcontext.expenses.Remove(result);
    }

    public void Update(Expense expense)
    {
        _dbcontext.expenses.Update(expense);
    }

    public async Task<List<Expense>> FilterByMonth(User user,DateOnly date)
    {
        
        var startDate = new DateTime(year: date.Year, month: date.Month, day: 1).Date;
        var daysInMonth = DateTime.DaysInMonth(year: date.Year, month: date.Month);
        var endDate = new DateTime(year: date.Year, month: date.Month, day: daysInMonth, hour: 23, minute: 59, second: 59);
        

        return await _dbcontext
            .expenses
            .AsNoTracking()
            .Where(e => e.UserId == user.Id && e.Date >= startDate && e.Date <= endDate)
            .OrderBy(e => e.Date)
            .ThenBy(e => e.Title)
            .ToListAsync();
    }
}
