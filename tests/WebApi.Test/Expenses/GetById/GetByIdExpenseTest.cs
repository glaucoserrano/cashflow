using CashFlow.Communication.Enums;
using CashFlow.Exception;
using CoommonTestsUtilities.Requests;
using FluentAssertions;
using System.Globalization;
using System.Net;
using System.Text.Json;
using WebApi.Test.InLineData;

namespace WebApi.Test.Expenses.GetById;
public class GetByIdExpenseTest : CashFlowClassFixture
{
    private const string METHOD = "api/Expenses";

    private readonly string _token;
    private readonly long _expenseId;
    public GetByIdExpenseTest(CustomWebApplicationFactory webApplicationFactory) : base(webApplicationFactory)
    {
        _token = webApplicationFactory.User_TeamMember.GetToken();
        _expenseId = webApplicationFactory.Expense_TeamMember.GetExpenseId();
    }

    [Fact(DisplayName = nameof(Sucess))]
    public async Task Sucess()
    {
        var result = await DoGet(requestUri: $"{METHOD}/{_expenseId}", token: _token);

        result.StatusCode.Should().Be(HttpStatusCode.OK);

        var body = await result.Content.ReadAsStreamAsync();

        var response = await JsonDocument.ParseAsync(body);

        response.RootElement.GetProperty("id").GetInt64().Should().Be(_expenseId);
        response.RootElement.GetProperty("title").GetString().Should().NotBeNullOrWhiteSpace();
        response.RootElement.GetProperty("description").GetString().Should().NotBeNullOrWhiteSpace();
        response.RootElement.GetProperty("date").GetDateTime().Should().NotBeAfter(DateTime.Today);
        response.RootElement.GetProperty("amount").GetDecimal().Should().BeGreaterThan(0);

        var paymentType = response.RootElement.GetProperty("paymentType").GetInt32();
        Enum.IsDefined(typeof(PaymentType), paymentType).Should().BeTrue();

    }
    //[Theory(DisplayName = nameof(Error_Expenses_Not_Found))]
    //[ClassData(typeof(CultureInLineDataTest))]
    //public async Task Error_Expenses_Not_Found(string cultureInfo)
    //{
    //    var request = RegisterExpenseValidatorTestsBuilder.Build();
    //    request.Title = string.Empty;

    //    var result = await DoPost(requestUri: METHOD, request: request, token: _token, cultureInfo: cultureInfo);

    //    result.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    //    var body = await result.Content.ReadAsStreamAsync();

    //    var response = await JsonDocument.ParseAsync(body);

    //    var errors = response.RootElement.GetProperty("errorMessages").EnumerateArray();

    //    var expectedMessage = ResourceErrorMessages.ResourceManager.GetString("TITLE_REQUIRED", new CultureInfo(cultureInfo));
    //    errors.Should().HaveCount(1).And.Contain(error => error.GetString()!.Equals(expectedMessage));
    //}

}
