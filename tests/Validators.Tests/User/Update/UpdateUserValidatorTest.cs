using CashFlow.Application.UseCases.Users.Update;
using CashFlow.Communication.Requests;
using CashFlow.Exception;
using CoommonTestsUtilities.Requests;
using FluentAssertions;

namespace Validators.Tests.User.Update;
public class UpdateUserValidatorTest
{
    [Fact(DisplayName = nameof(Sucess))]
    public void Sucess() 
    {
        var validator = new UpdateUserValidator();
        var request = ReuquestUpdateUserJsonBuilder.Build();

        var result = validator.Validate(request);

        result.IsValid.Should().BeTrue();
    }
    [Theory(DisplayName = nameof(Error_Name_Empty))]
    [InlineData("")]
    [InlineData("      ")]
    [InlineData(null)]
    public void Error_Name_Empty(string name)
    {
        var validator = new UpdateUserValidator();
        var request = ReuquestUpdateUserJsonBuilder.Build();
        request.Name = name;

        var result = validator.Validate(request);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle().And.Contain(e => e.ErrorMessage.Equals(ResourceErrorMessages.NAME_EMPTY));
    }

    [Theory(DisplayName = nameof(Error_Email_Empty))]
    [InlineData("")]
    [InlineData("      ")]
    [InlineData(null)]
    public void Error_Email_Empty(string email)
    {
        var validator = new UpdateUserValidator();
        var request = ReuquestUpdateUserJsonBuilder.Build();
        request.Email = email;

        var result = validator.Validate(request);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle().And.Contain(e => e.ErrorMessage.Equals(ResourceErrorMessages.EMAIL_EMPTY));
    }
    [Fact(DisplayName = nameof(Error_Email_Invalid))]
    public void Error_Email_Invalid()
    {
        var validator = new UpdateUserValidator();
        var request = ReuquestUpdateUserJsonBuilder.Build();
        request.Email = "glauco.com";

        var result = validator.Validate(request);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle().And.Contain(e => e.ErrorMessage.Equals(ResourceErrorMessages.EMAIL_INVALID));

    }
}
