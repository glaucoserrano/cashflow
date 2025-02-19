using CashFlow.Application.UseCases.Expenses.GetById;
using CashFlow.Domain.Entities;
using CashFlow.Exception.ExceptionsBase;
using CashFlow.Exception;
using CoommonTestsUtilities.Entities;
using CoommonTestsUtilities.Mapper;
using CoommonTestsUtilities.Repositories;
using CoommonTestsUtilities;
using FluentAssertions;
using CashFlow.Application.UseCases.Expenses.Delete;

namespace UseCases.Test.Expenses.Delete;
public class DeleteExpenseUseCaseTest
{
    [Fact(DisplayName = nameof(Sucess))]
    public async Task Sucess()
    {
        var loggedUser = UserBuilder.Build();
        var expense = ExpenseBuilder.Build(user: loggedUser);

        var useCase = CreateUseCase(user: loggedUser, expense: expense);

        var act = async () => await useCase.Execute(expense.Id);

        await act.Should().NotThrowAsync();

    }
    [Fact(DisplayName = nameof(Error_Expense_Not_Found))]
    public async Task Error_Expense_Not_Found()
    {
        var loggedUser = UserBuilder.Build();
        var useCase = CreateUseCase(user: loggedUser);

        var act = async () => await useCase.Execute(id: 1000);

        var result = await act.Should().ThrowAsync<NotFoundExcepption>();

        result.Where(ex => ex.GetErrors().Count == 1 && ex.GetErrors().Contains(ResourceErrorMessages.EXPENSE_NOT_FOUND));
    }

    private DeleteExpenseUseCase CreateUseCase(User user, Expense? expense = null)
    {
        var respositoryWriteOnly = ExpensesWriteOnlyRepositoryBuilder.Build();
        var repository = new ExpensesReadOnlyRepositoryBuilder().GetById(user: user, expense: expense).Build();
        var unitOfWork = UnitOfWorkBuilder.Build();
        var loggedUser = LoggedUserBuilder.Build(user);

        return new DeleteExpenseUseCase(repository: respositoryWriteOnly, unitOfWQork: unitOfWork, loggedUser: loggedUser, expensesReadOnly: repository);


    }
}
