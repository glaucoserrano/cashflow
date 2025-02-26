﻿using CashFlow.Exception;
using FluentAssertions;
using System.Globalization;
using System.Net;
using System.Text.Json;
using WebApi.Test.InLineData;

namespace WebApi.Test.Expenses.Delete;
public class DeleteExpenseTest : CashFlowClassFixture
{
    private const string METHOD = "api/Expenses";

    private readonly string _token;
    private readonly long _expenseId;
    public DeleteExpenseTest(CustomWebApplicationFactory webApplicationFactory) : base(webApplicationFactory)
    {
        _token = webApplicationFactory.User_TeamMember.GetToken();
        _expenseId = webApplicationFactory.Expense_TeamMember.GetExpenseId();
    }

    [Fact(DisplayName = nameof(Sucess))]
    public async Task Sucess()
    {
        var result = await DoDelete(requestUri: $"{METHOD}/{_expenseId}", token: _token);

        result.StatusCode.Should().Be(HttpStatusCode.NoContent);

        result = await DoGet(requestUri: $"{METHOD}/{_expenseId}", token: _token);

        result.StatusCode.Should().Be(HttpStatusCode.NotFound);

    }
    [Theory(DisplayName = nameof(Error_Expenses_Not_Found))]
    [ClassData(typeof(CultureInLineDataTest))]
    public async Task Error_Expenses_Not_Found(string cultureInfo)
    {
        var result = await DoDelete(requestUri: $"{METHOD}/1000", token: _token,cultureInfo: cultureInfo);

        result.StatusCode.Should().Be(HttpStatusCode.NotFound);
        var body = await result.Content.ReadAsStreamAsync();

        var response = await JsonDocument.ParseAsync(body);

        var errors = response.RootElement.GetProperty("errorMessages").EnumerateArray();

        var expectedMessage = ResourceErrorMessages.ResourceManager.GetString("EXPENSE_NOT_FOUND", new CultureInfo(cultureInfo));
        errors.Should().HaveCount(1).And.Contain(error => error.GetString()!.Equals(expectedMessage));
    }
}
