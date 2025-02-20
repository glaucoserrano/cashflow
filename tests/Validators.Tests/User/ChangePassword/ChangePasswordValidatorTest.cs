using CashFlow.Application.UseCases.Users.ChangePassword;
using CashFlow.Exception;
using CoommonTestsUtilities.Requests;
using FluentAssertions;

namespace Validators.Tests.User.ChangePassword;
public class ChangePasswordValidatorTest
{
    [Fact(DisplayName = nameof(Sucess))]
    public void Sucess()
    {
        var validator = new ChangePasswordValidator();
        var request = RequestChangePasswordJsonBuilder.Build();

        var result = validator.Validate(request);

        result.IsValid.Should().BeTrue();
    }
    [Theory(DisplayName = nameof(Error_NewPassword_Empty))]
    [InlineData("")]
    [InlineData("           ")]
    [InlineData(null)]
    public void Error_NewPassword_Empty(string newPassword)
    {

        var validator = new ChangePasswordValidator();
        var request = RequestChangePasswordJsonBuilder.Build();
        request.NewPassword = newPassword;

        var result = validator.Validate(request);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle().And.Contain(e => e.ErrorMessage.Equals(ResourceErrorMessages.INVALID_PASSWORD));

    }
}
