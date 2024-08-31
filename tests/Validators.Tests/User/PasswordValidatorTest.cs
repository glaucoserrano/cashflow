using CashFlow.Application.UseCases.Users;
using CashFlow.Communication.Requests;
using CoommonTestsUtilities.Requests;
using FluentAssertions;
using FluentValidation;

namespace Validators.Tests.User;
public class PasswordValidatorTest
{
    [Theory(DisplayName = nameof(Error_Password_Invalid))]
    [InlineData("")]
    [InlineData("           ")]
    [InlineData(null)]
    [InlineData("a")]
    [InlineData("aa")]
    [InlineData("aaa")]
    [InlineData("aaaa")]
    [InlineData("aaaaa")]
    [InlineData("aaaaaa")]
    [InlineData("aaaaaaa")]
    [InlineData("aaaaaaaa")]
    [InlineData("Aaaaaaaa")]
    [InlineData("Aaaaaaa1")]

    public void Error_Password_Invalid(string password)
    {

        var validator = new PasswordValidator<RequestRegisterJson>();

        var result = validator
            .IsValid(new ValidationContext<RequestRegisterJson>(new RequestRegisterJson()),password);

        result.Should().BeFalse();

    }
}
