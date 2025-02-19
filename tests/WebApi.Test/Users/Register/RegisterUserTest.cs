using CashFlow.Exception;
using CoommonTestsUtilities.Requests;
using FluentAssertions;
using System.Globalization;
using System.Net;
using System.Text.Json;
using WebApi.Test.InLineData;

namespace WebApi.Test.Users.Register;
public class RegisterUserTest : CashFlowClassFixture
{
    private const string METHOD = "api/User";
    

    public RegisterUserTest(CustomWebApplicationFactory webApplicationFactory) : base(webApplicationFactory)
    {
        
    }

   
    [Fact(DisplayName =nameof(Sucess))]
    public async Task Sucess()
    {
        var request = RequestREgisterUserJsonBuilder.Build();
        var result = await DoPost(requestUri: METHOD, request);

        result.StatusCode.Should().Be(HttpStatusCode.Created);
        var body = await result.Content.ReadAsStreamAsync();

        var response = await JsonDocument.ParseAsync(body);

        response.RootElement.GetProperty("name").GetString().Should().Be(request.Name);
        response.RootElement.GetProperty("token").GetString().Should().NotBeNullOrEmpty();

    }
    [Theory(DisplayName = nameof(Error_Empty_Name))]
    [ClassData(typeof(CultureInLineDataTest))]
    public async Task Error_Empty_Name(string cultureInfo)
    {
        var request = RequestREgisterUserJsonBuilder.Build();
        request.Name = string.Empty;

        
        var result = await DoPost(requestUri: METHOD, request: request,cultureInfo: cultureInfo);

        result.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        var body = await result.Content.ReadAsStreamAsync();

        var response = await JsonDocument.ParseAsync(body);

        var errors = response.RootElement.GetProperty("errorMessages").EnumerateArray();

        var expectedMessage = ResourceErrorMessages.ResourceManager.GetString("NAME_EMPTY", new CultureInfo(cultureInfo));
        errors.Should().HaveCount(1).And.Contain(error => error.GetString()!.Equals(expectedMessage));
    }
}
