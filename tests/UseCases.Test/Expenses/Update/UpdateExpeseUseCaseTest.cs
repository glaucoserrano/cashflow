using CashFlow.Application.UseCases.Expenses.Update;
using CashFlow.Domain.Entities;
using CashFlow.Exception.ExceptionsBase;
using CashFlow.Exception;
using CoommonTestsUtilities.Entities;
using CoommonTestsUtilities.Mapper;
using CoommonTestsUtilities.Repositories;
using CoommonTestsUtilities.Requests;
using CoommonTestsUtilities;
using FluentAssertions;
using CashFlow.Domain.Enum;

namespace UseCases.Test.Expenses.Update;
public class UpdateExpeseUseCaseTest
{
    [Fact(DisplayName = nameof(Sucess))]
    public async Task Sucess()
    {
        var loggedUser = UserBuilder.Build();
        var request = RegisterExpenseValidatorTestsBuilder.Build();

        var expense = ExpenseBuilder.Build(user: loggedUser);

        var useCase = CreateUseCase(user: loggedUser, expense: expense);

        var act = async () => await useCase.Execute(expense.Id, request);

        await act.Should().NotThrowAsync();

        expense.Title.Should().Be(request.Title);
        expense.Description.Should().Be(request.Description);
        expense.Date.Should().Be(request.Date);
        expense.Amount.Should().Be(request.Amount);
        expense.PaymentType.Should().Be((PaymentType)expense.PaymentType);


    }

    [Fact(DisplayName = nameof(Error_Title_Empty))]
    public async Task Error_Title_Empty()
    {
        var loggedUser = UserBuilder.Build();
        var expense = ExpenseBuilder.Build(user: loggedUser);

        var request = RegisterExpenseValidatorTestsBuilder.Build();
        request.Title = string.Empty;

        var useCase = CreateUseCase(loggedUser, expense);

        var act = async () => await useCase.Execute(id: expense.Id, request: request);

        var result = await act.Should().ThrowAsync<ErrorOnValidateException>();

        result.Where(ex => ex.GetErrors().Count == 1 && ex.GetErrors().Contains(ResourceErrorMessages.TITLE_REQUIRED));
    }
    [Fact(DisplayName = nameof(Error_Expense_Not_Found))]
    public async Task Error_Expense_Not_Found()
    {
        var loggedUser = UserBuilder.Build();
        var request = RegisterExpenseValidatorTestsBuilder.Build();

        var useCase = CreateUseCase(user: loggedUser);

        var act = async () => await useCase.Execute(id: 1000, request);

        var result = await act.Should().ThrowAsync<NotFoundExcepption>();

        result.Where(ex => ex.GetErrors().Count == 1 && ex.GetErrors().Contains(ResourceErrorMessages.EXPENSE_NOT_FOUND));
    }

    private UpdateExpenseUseCase CreateUseCase(User user, Expense? expense = null)
    {
        var repository = new ExpensesUpdateRepositoryBuilder().GetById(user: user, expense: expense).Build();
        var mapper = MapperBuilder.Build();
        var unitOfWork = UnitOfWorkBuilder.Build();
        var loggedUser = LoggedUserBuilder.Build(user);

        return new UpdateExpenseUseCase(mapper: mapper, unitOfWork: unitOfWork, repository: repository, loggedUser: loggedUser);

    }
}
