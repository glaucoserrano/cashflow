using CashFlow.Application.UseCases.Expenses;
using CashFlow.Communication.Enums;
using CashFlow.Exception;
using CoommonTestsUtilities.Requests;
using FluentAssertions;
using Xunit;

namespace Validators.Tests.Expenses.Register;
public class RegisterExpenseValidatorTests
{
    [Fact(DisplayName = nameof(Sucess))]
    public void Sucess()
    {
        var validator = new RegisterExpenseValidator();
        var request = RegisterExpenseValidatorTestsBuilder.Build();

        var result = validator.Validate(request);

        result.IsValid.Should().BeTrue();
    }
    [Theory(DisplayName =nameof(Error_Title_Empty))]
    [InlineData("")]
    [InlineData("          ")]
    [InlineData(null)]
    public void Error_Title_Empty(string title) 
    {
        var validator = new RegisterExpenseValidator();
        var request = RegisterExpenseValidatorTestsBuilder.Build();

        request.Title = title;

        var result = validator.Validate(request);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle().And.Contain(e => e.ErrorMessage.Equals(ResourceErrorMessages.TITLE_REQUIRED));
    }
    [Fact(DisplayName = nameof(Error_Date_Future))]
    public void Error_Date_Future()
    {
        var validator = new RegisterExpenseValidator();
        var request = RegisterExpenseValidatorTestsBuilder.Build();

        request.Date = DateTime.Now.AddDays(1);

        var result = validator.Validate(request);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle().And.Contain(e => e.ErrorMessage.Equals(ResourceErrorMessages.EXPANSES_CANNOT_FOR_THE_FUTURE));
    }
    [Fact(DisplayName = nameof(Error_Payment_Type_Invalid))]
    public void Error_Payment_Type_Invalid()
    {
        var validator = new RegisterExpenseValidator();
        var request = RegisterExpenseValidatorTestsBuilder.Build();

        request.PaymentType = (PaymentType)700;

        var result = validator.Validate(request);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle().And.Contain(e => e.ErrorMessage.Equals(ResourceErrorMessages.PAYMENT_TYPE_INVALID));
    }

    [Theory(DisplayName = nameof(Error_Amount_Invalid))]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(-2)]
    [InlineData(-7)]
    public void Error_Amount_Invalid(decimal amount)
    {
        var validator = new RegisterExpenseValidator();
        var request = RegisterExpenseValidatorTestsBuilder.Build();

        request.Amount = amount;

        var result = validator.Validate(request);

        result.IsValid.Should().BeFalse();
        result.Errors.Should().ContainSingle().And.Contain(e => e.ErrorMessage.Equals(ResourceErrorMessages.AMOUNT_MUST_BE_GREATER_THAN_ZERO));
    }
}
