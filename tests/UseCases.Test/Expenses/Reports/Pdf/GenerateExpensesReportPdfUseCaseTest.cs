using CashFlow.Application.UseCases.Expenses.Reports.PDF;
using CashFlow.Domain.Entities;
using CoommonTestsUtilities;
using CoommonTestsUtilities.Entities;
using CoommonTestsUtilities.Repositories;
using FluentAssertions;

namespace UseCases.Test.Expenses.Reports.Pdf;
public class GenerateExpensesReportPdfUseCaseTest
{
    [Fact(DisplayName = nameof(Sucess))]
    public async Task Sucess()
    {
        var loggedUser = UserBuilder.Build();
        var expenses = ExpenseBuilder.Collection(loggedUser);

        var useCase = CreateUseCase(user: loggedUser, expenses: expenses);

        var result = await useCase.Execute(DateOnly.FromDateTime(DateTime.Today));

        result.Should().NotBeNullOrEmpty();
    }

    [Fact(DisplayName = nameof(Sucess_Empty))]
    public async Task Sucess_Empty() 
    {
        var loggedUser = UserBuilder.Build();

        var useCase = CreateUseCase(user: loggedUser, expenses: new List<Expense>());

        var result = await useCase.Execute(DateOnly.FromDateTime(DateTime.Today));

        result.Should().BeEmpty();
    }

    private GenerateExpensesReportPdfUseCase CreateUseCase(User user, List<Expense> expenses)
    {
        var repository = new ExpensesReadOnlyRepositoryBuilder().FilterByMonth(user: user, expenses: expenses).Build();
        var loggedUser = LoggedUserBuilder.Build(user);

        return new GenerateExpensesReportPdfUseCase(repository: repository, loggedUser: loggedUser);
    }
}
