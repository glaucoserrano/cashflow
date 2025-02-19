using CashFlow.Exception;
using CoommonTestsUtilities.Requests;
using FluentAssertions;
using System.Globalization;
using System.Net;
using System.Text.Json;
using WebApi.Test.InLineData;

namespace WebApi.Test.Expenses.Update;
public class UpdateExpensesTest: CashFlowClassFixture
{
    private const string METHOD = "api/Expenses";

    private readonly string _token;
    private readonly long _expenseId;

    public UpdateExpensesTest(CustomWebApplicationFactory webApplicationFactory) : base(webApplicationFactory)
    {
        _token = webApplicationFactory.User_TeamMember.GetToken();
        _expenseId = webApplicationFactory.Expense_TeamMember.GetExpenseId();
    }
    [Fact]
    public async Task Sucess()
    {
        var request = RegisterExpenseValidatorTestsBuilder.Build();

        var result = await DoPut(requestUri: $"{METHOD}/{_expenseId}",request: request, token: _token);

        result.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }
    [Theory(DisplayName = nameof(Error_Title_Empty))]
    [ClassData(typeof(CultureInLineDataTest))]
    public async Task Error_Title_Empty(string cultureInfo)
    {
        var request = RegisterExpenseValidatorTestsBuilder.Build();
        request.Title = string.Empty;

        var result = await DoPut(requestUri: $"{METHOD}/{_expenseId}", request: request, token: _token, cultureInfo: cultureInfo);

        result.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        var body = await result.Content.ReadAsStreamAsync();

        var response = await JsonDocument.ParseAsync(body);

        var errors = response.RootElement.GetProperty("errorMessages").EnumerateArray();

        var expectedMessage = ResourceErrorMessages.ResourceManager.GetString("TITLE_REQUIRED", new CultureInfo(cultureInfo));
        errors.Should().HaveCount(1).And.Contain(error => error.GetString()!.Equals(expectedMessage));
    }
    [Theory(DisplayName = nameof(Error_Expenses_Not_Found))]
    [ClassData(typeof(CultureInLineDataTest))]
    public async Task Error_Expenses_Not_Found(string cultureInfo)
    {
        var request = RegisterExpenseValidatorTestsBuilder.Build();
        var result = await DoPut(requestUri: $"{METHOD}/1000",request: request, token: _token, cultureInfo: cultureInfo);

        result.StatusCode.Should().Be(HttpStatusCode.NotFound);
        var body = await result.Content.ReadAsStreamAsync();

        var response = await JsonDocument.ParseAsync(body);

        var errors = response.RootElement.GetProperty("errorMessages").EnumerateArray();

        var expectedMessage = ResourceErrorMessages.ResourceManager.GetString("EXPENSE_NOT_FOUND", new CultureInfo(cultureInfo));
        errors.Should().HaveCount(1).And.Contain(error => error.GetString()!.Equals(expectedMessage));
    }
}
