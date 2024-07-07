using CashFlow.Domain.Entities;

namespace CashFlow.Domain.Repositories.Expenses;
public interface IExpensesWriteOnlyRepository
{
    Task Add(Expense expenses);
    /// <summary>
    /// This fuynction returns true if trhe deletion was sucessful
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<bool> Delete(long id);
}
