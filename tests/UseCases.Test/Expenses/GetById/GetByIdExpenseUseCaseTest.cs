using CashFlow.Application.UseCases.Expenses.GetById;
using CashFlow.Domain.Entities;
using CashFlow.Communication.Enums;
using CashFlow.Exception;
using CashFlow.Exception.ExceptionsBase;
using CoommonTestsUtilities;
using CoommonTestsUtilities.Entities;
using CoommonTestsUtilities.Mapper;
using CoommonTestsUtilities.Repositories;
using FluentAssertions;

namespace UseCases.Test.Expenses.GetById;
public class GetByIdExpenseUseCaseTest
{
    [Fact(DisplayName =nameof(Sucess))]
    public async Task Sucess()
    {
        var loggedUser = UserBuilder.Build();
        var expense = ExpenseBuilder.Build(user: loggedUser);

        var useCase = CreateUseCase(user: loggedUser, expense: expense);

        var result = await useCase.Execute(expense.Id);

        result.Should().NotBeNull();
        result.ID.Should().Be(expense.Id); 
        result.Title.Should().Be(expense.Title); 
        result.Description.Should().Be(expense.Description); 
        result.Amount.Should().Be(expense.Amount); 
        result.PaymentType.Should().Be((PaymentType)expense.PaymentType);
        result.Tags.Should().NotBeEmpty().And.BeEquivalentTo(expense.Tags.Select(tag => tag.Value));

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

    private GetExpenseByIdUseCase CreateUseCase(User user, Expense? expense = null)
    {
        var repository = new ExpensesReadOnlyRepositoryBuilder().GetById(user: user,expense: expense).Build();
        var mapper = MapperBuilder.Build();
        var loggedUser = LoggedUserBuilder.Build(user);

        return new GetExpenseByIdUseCase(repository: repository, mapper: mapper, loggedUser: loggedUser);

    }
}
