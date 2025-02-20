using CashFlow.Application.UseCases.Users.Delete;
using CashFlow.Domain.Entities;
using CoommonTestsUtilities;
using CoommonTestsUtilities.Entities;
using CoommonTestsUtilities.Repositories;
using FluentAssertions;

namespace UseCases.Test.Users.Delete;
public class DeleteUserAccountUseCaseTest
{
    [Fact(DisplayName =nameof(Sucess))]
    public async Task Sucess()
    {
        var user = UserBuilder.Build();
        var useCase = CreateUseCase(user);

        var act = async () => await useCase.Execute();

        await act.Should().NotThrowAsync();

    }
    private DeleteUserAccountUseCase CreateUseCase(User user)
    {
        var repository = UserWriteOnlyRepositoryBuilder.Build();
        var loggedUser = LoggedUserBuilder.Build(user);
        var unitOfWork = UnitOfWorkBuilder.Build();

        return new DeleteUserAccountUseCase(loggedUser, repository, unitOfWork);
    }
}
