﻿using CashFlow.Domain.Repositories;
using CashFlow.Domain.Repositories.Expenses;
using CashFlow.Exception;
using CashFlow.Exception.ExceptionsBase;

namespace CashFlow.Application.UseCases.Expenses.Delete;
public class DeleteExpenseUseCase : IDeleteExpenseUseCase
{
    private readonly IExpensesWriteOnlyRepository _repository;
    private readonly IUnitOfWork _unitOfWork;
    public DeleteExpenseUseCase(IExpensesWriteOnlyRepository repository,IUnitOfWork unitOfWQork)
    {
        _repository = repository;
        _unitOfWork = unitOfWQork;
    }

    public async Task Execute(long id)
    {
        var result = await _repository.Delete(id);

        if(result == false)
        {
            throw new NotFoundExcepption(ResourceErrorMessages.EXPENSE_NOT_FOUND);
        }
        await _unitOfWork.Commit();
    }
}
