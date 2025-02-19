using CashFlow.Domain.Repositories;
using CashFlow.Domain.Repositories.Expenses;
using CashFlow.Domain.Services.LoggedUser;
using CashFlow.Exception;
using CashFlow.Exception.ExceptionsBase;

namespace CashFlow.Application.UseCases.Expenses.Delete;
public class DeleteExpenseUseCase : IDeleteExpenseUseCase
{
    private readonly IExpensesWriteOnlyRepository _repository;
    private readonly IExpensesReadOnlyRepository _expensesReadOnly;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILoggedUser _loggedUser;
    public DeleteExpenseUseCase(
        IExpensesWriteOnlyRepository repository,
        IUnitOfWork unitOfWQork,
        ILoggedUser loggedUser,
        IExpensesReadOnlyRepository expensesReadOnly)
    {
        _repository = repository;
        _unitOfWork = unitOfWQork;
        _loggedUser = loggedUser;
        _expensesReadOnly = expensesReadOnly;
    }

    public async Task Execute(long id)
    {
        var loggedUser = await _loggedUser.Get();

        var expense = await _expensesReadOnly.GetById(loggedUser,id);

        if(expense is null)
        {
            throw new NotFoundExcepption(ResourceErrorMessages.EXPENSE_NOT_FOUND);
        }
        await _repository.Delete(id);

        await _unitOfWork.Commit();
    }
}
