using CashFlow.Application.UseCases.Users.Update;
using CashFlow.Domain.Entities;
using CashFlow.Exception;
using CashFlow.Exception.ExceptionsBase;
using CoommonTestsUtilities;
using CoommonTestsUtilities.Entities;
using CoommonTestsUtilities.Repositories;
using CoommonTestsUtilities.Requests;
using FluentAssertions;

namespace UseCases.Test.Users.Update;
public class UpdateUserUseCaseTest
{
    [Fact(DisplayName =nameof(Sucess))]
    public async Task Sucess()
    {
        var user = UserBuilder.Build();
        var request = ReuquestUpdateUserJsonBuilder.Build();

        var useCase = CreateUseCase(user);

        var act = async () => await useCase.Execute(request);

        await act.Should().NotThrowAsync();

        user.Name.Should().Be(request.Name);
        user.Email.Should().Be(request.Email);
    }
    [Fact(DisplayName = nameof(Error_Name_Empty))]
    public async Task Error_Name_Empty()
    {
        var user = UserBuilder.Build();
        var request = ReuquestUpdateUserJsonBuilder.Build();
        request.Name = string.Empty;

        var useCase = CreateUseCase(user);

        var act = async () => await useCase.Execute(request);

        var result = await act.Should().ThrowAsync<ErrorOnValidateException>();

        result.Where(ex => ex.GetErrors().Count == 1 && ex.GetErrors().Contains(ResourceErrorMessages.NAME_EMPTY));

    }

    private UpdateUserUseCase CreateUseCase(User user, string? email = null)
    {
        var unitOfWork = UnitOfWorkBuilder.Build();
        var updateRespository = UserUpdateOnlyRepositoryBuilder.Build(user);
        var loggedUser = LoggedUserBuilder.Build(user);
        var readrepository = new UserReadOnlyRepositoryBuilder();

        if (string.IsNullOrWhiteSpace(email) == false)
            readrepository.ExisrActiveUserWithEmail(email);

        return new UpdateUserUseCase(loggedUser, updateRespository, readrepository.Build(), unitOfWork);
    }
}
