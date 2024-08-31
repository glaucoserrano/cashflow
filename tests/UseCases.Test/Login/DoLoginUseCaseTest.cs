using CashFlow.Application.UseCases.Users.Login;
using CashFlow.Domain.Entities;
using CoommonTestsUtilities.Cryptography;
using CoommonTestsUtilities.Entities;
using CoommonTestsUtilities.Repositories;
using CoommonTestsUtilities.Requests;
using CoommonTestsUtilities.Token;
using FluentAssertions;

namespace UseCases.Test.Login;
public class DoLoginUseCaseTest
{
    [Fact(DisplayName = nameof(Sucess))]
    public async Task Sucess() 
    {
        var request = RequestLoginJsonBuilder.Build();
        var user = UserBuilder.Build();

        var useCase = CreateUseCase(user);

        var result = await useCase.Execute(request);

        result.Should().NotBeNull();
        result.Name.Should().Be(user.Name);
        result.Token.Should().NotBeNullOrWhiteSpace();
    }

    [Fact(DisplayName = nameof(Error_User_NotFound))]
    public async Task Error_User_NotFound() { }

    [Fact(DisplayName = nameof(Error_Password_Not_Match))]
    public async Task Error_Password_Not_Match() { }


    private DoLoginUseCase CreateUseCase(User user)
    {
        var passwordEncripter = PasswordEncripterBuilder.Builder();
        var tokenGeneration = JwtTokenGeneratorBuilder.Build();
        var readOnlyRepository = new UserReadOnlyRepositoryBuilder();

        readOnlyRepository.GetUserByEmail(user);


        return new DoLoginUseCase(readOnlyRepository.Build(), passwordEncripter, tokenGeneration);
    }
}
