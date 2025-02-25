using CashFlow.Domain.Entities;
using CashFlow.Domain.Enum;
using CashFlow.Domain.Security.Cryptography;
using CashFlow.Domain.Security.Token;
using CashFlow.Infrastructure.DataAcess;
using CoommonTestsUtilities.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using WebApi.Test.Resources;

namespace WebApi.Test;
public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{



    public UserIdentityManager User_TeamMember { get; private set; } = default!;
    public UserIdentityManager User_Admin { get; private set; } = default!;
    public ExpensesIdentityManager Expense_TeamMember { get; private set; } = default!;
    public ExpensesIdentityManager Expense_Admin { get; private set; } = default!;


    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Test")
            .ConfigureServices(services =>
            {
                var provider = services.AddEntityFrameworkInMemoryDatabase().BuildServiceProvider();
                services.AddDbContext<CashFlowDbContext>(config =>
                {
                    config.UseInMemoryDatabase("InMemoryDbForTesting");
                    config.UseInternalServiceProvider(provider);
                });

                var scope = services.BuildServiceProvider().CreateScope();
                var dbContext = scope.ServiceProvider.GetRequiredService<CashFlowDbContext>();
                var passwordEncripter = scope.ServiceProvider.GetRequiredService<IPasswordEmcripter>();
                var tokenGenerator = scope.ServiceProvider.GetRequiredService<IAccessTokenGenerator>();


                StartDataBase(dbContext, passwordEncripter,tokenGenerator);
               
            });
    }
    
    private void StartDataBase(CashFlowDbContext dbContext, IPasswordEmcripter passwordEmcripter, IAccessTokenGenerator tokenGenerator)
    {
        var userTeamMember =  AddUsersTeamMember(dbContext, passwordEmcripter, tokenGenerator);
        var expenseTeamMember =  AddExpenses(dbContext, userTeamMember, expenseId: 1, tagId: 1);
        Expense_TeamMember = new ExpensesIdentityManager(expenseTeamMember);

        var userAdmin = AddUsersAdmin(dbContext, passwordEmcripter, tokenGenerator);
        var expenseAdmin = AddExpenses(dbContext, userAdmin,expenseId: 2, tagId: 2);
        Expense_Admin = new ExpensesIdentityManager(expenseAdmin);

        dbContext.SaveChanges();
    }
    private User AddUsersTeamMember(
        CashFlowDbContext dbContext, 
        IPasswordEmcripter passwordEmcripter, 
        IAccessTokenGenerator accessTokenGenerator
        )
    {
        var user = UserBuilder.Build();
        user.Id = 1;

        var password = user.Password;

        user.Password = passwordEmcripter.Encrypt(user.Password);
        dbContext.users.Add(user);


        var token = accessTokenGenerator.Generate(user);
        User_TeamMember = new UserIdentityManager(user: user, password: password, token: token);
        return user;
    }
    private User AddUsersAdmin(
        CashFlowDbContext dbContext,
        IPasswordEmcripter passwordEmcripter,
        IAccessTokenGenerator accessTokenGenerator
        )
    {
        var user = UserBuilder.Build(role: Roles.ADMIN);
        user.Id = 2;

        var password = user.Password;

        user.Password = passwordEmcripter.Encrypt(user.Password);
        dbContext.users.Add(user);


        var token = accessTokenGenerator.Generate(user);
        User_Admin = new UserIdentityManager(user: user, password: password, token: token);
        return user;
    }
    private Expense AddExpenses(CashFlowDbContext dbContext, User user, long expenseId, long tagId)
    {
         var expense = ExpenseBuilder.Build(user);
        expense.Id = expenseId;

        foreach (var tag in expense.Tags)
        {
            tag.Id = tagId;
            tag.ExpenseId = expenseId;
        }

        dbContext.expenses.Add(expense);


        return expense;

    }
}
