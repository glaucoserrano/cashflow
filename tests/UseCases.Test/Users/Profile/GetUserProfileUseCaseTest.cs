using CashFlow.Application.UseCases.Users.Profile;
using CashFlow.Domain.Entities;
using CoommonTestsUtilities;
using CoommonTestsUtilities.Entities;
using CoommonTestsUtilities.Mapper;
using FluentAssertions;
using System.ComponentModel.DataAnnotations;

namespace UseCases.Test.Users.Profile;
public class GetUserProfileUseCaseTest
{
    [Fact(DisplayName = nameof(Sucess))]
    public async Task Sucess()
    {
        var user = UserBuilder.Build();
        var useCase = CreateUserCase(user);

        var result = await useCase.Execute();

        result.Should().NotBeNull();
        result.Name.Should().Be(user.Name);
        result.Email.Should().Be(user.Email);
    }

    private GetUserProfileUseCase CreateUserCase(User user)
    {
        var mapper = MapperBuilder.Build();
        var loggedUser = LoggedUserBuilder.Build(user);

        return new GetUserProfileUseCase(loggedUser, mapper);
    }

}
