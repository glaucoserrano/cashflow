using CashFlow.Communication.Requests;
using CashFlow.Exception;
using CoommonTestsUtilities.Requests;
using FluentAssertions;
using System.Globalization;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using WebApi.Test.InLineData;

namespace WebApi.Test.Login.DoLogin;
public class DoLoginTest : CashFlowClassFixture
{
    private const string METHOD = "api/Login";

    private readonly string _email;
    private readonly string _name;
    private readonly string _password;

    public DoLoginTest(CustomWebApplicationFactory webApplicationFactory) : base(webApplicationFactory)
    {
        _email = webApplicationFactory.User_TeamMember.GetEmail();
        _name = webApplicationFactory.User_TeamMember.GetName();
        _password = webApplicationFactory.User_TeamMember.GetPassword();
    }

    [Fact(DisplayName = nameof(Success))]
    public async Task Success()
    {
        var request = new RequestLoginJson
        {
            Email = _email,
            Password = _password
        };

        var response = await DoPost(requestUri: METHOD, request: request);

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var responseBody = await response.Content.ReadAsStreamAsync();

        var responseData = await JsonDocument.ParseAsync(responseBody);

        responseData.RootElement.GetProperty("name").GetString().Should().Be(_name);
        responseData.RootElement.GetProperty("token").GetString().Should().NotBeNullOrWhiteSpace();

    }
    [Theory(DisplayName = nameof(Error_Login_Invalid))]
    [ClassData(typeof(CultureInLineDataTest))]
    public async Task Error_Login_Invalid(string cultureInfo)
    {
        var request = RequestREgisterUserJsonBuilder.Build();

        var result = await DoPost(requestUri: METHOD, request: request,cultureInfo: cultureInfo);

        result.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        var body = await result.Content.ReadAsStreamAsync();

        var response = await JsonDocument.ParseAsync(body);

        var errors = response.RootElement.GetProperty("errorMessages").EnumerateArray();

        var expectedMessage = ResourceErrorMessages.ResourceManager.GetString("EMAIL_OR_PASSWORD_INVALID", new CultureInfo(cultureInfo));
        errors.Should().HaveCount(1).And.Contain(error => error.GetString()!.Equals(expectedMessage));
    }
}
