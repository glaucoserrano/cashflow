using CashFlow.Application.UseCases.Users;
using CashFlow.Exception;
using CoommonTestsUtilities.Requests;
using FluentAssertions;

namespace Validators.Tests.User.Register;
public class RegisterUserValidatorTest
{
    [Fact(DisplayName = nameof(Sucess))]
    public void Sucess()
    {

        var validator = new RegisterUserValidator();
        var request = RequestREgisterUserJsonBuilder.Build();

        var result = validator.Validate(request);

        result.IsValid.Should().BeTrue();

    }
    [Theory(DisplayName = nameof(Error_Name_Empty))]
    [InlineData("")]
    [InlineData("           ")]
    [InlineData(null)]
    public void Error_Name_Empty(string name)
    {

        var validator = new RegisterUserValidator();
        var request = RequestREgisterUserJsonBuilder.Build();
        request.Name = name;

        var result = validator.Validate(request);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle().And.Contain(e => e.ErrorMessage.Equals(ResourceErrorMessages.NAME_EMPTY));

    }
    [Theory(DisplayName = nameof(Error_Email_Empty))]
    [InlineData("")]
    [InlineData("           ")]
    [InlineData(null)]
    public void Error_Email_Empty(string email)
    {

        var validator = new RegisterUserValidator();
        var request = RequestREgisterUserJsonBuilder.Build();
        request.Email = email;

        var result = validator.Validate(request);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle().And.Contain(e => e.ErrorMessage.Equals(ResourceErrorMessages.EMAIL_EMPTY));

    }
    [Fact(DisplayName = nameof(Error_Email_Invalid))]
    public void Error_Email_Invalid()
    {

        var validator = new RegisterUserValidator();
        var request = RequestREgisterUserJsonBuilder.Build();
        request.Email = "email.com";

        var result = validator.Validate(request);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle().And.Contain(e => e.ErrorMessage.Equals(ResourceErrorMessages.EMAIL_INVALID));

    }
    [Fact(DisplayName = nameof(Error_Password_Empty))]
    public void Error_Password_Empty()
    {

        var validator = new RegisterUserValidator();
        var request = RequestREgisterUserJsonBuilder.Build();
        request.Password = string.Empty;

        var result = validator.Validate(request);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle().And.Contain(e => e.ErrorMessage.Equals(ResourceErrorMessages.INVALID_PASSWORD));

    }
}
