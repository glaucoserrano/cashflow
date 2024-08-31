using Bogus;
using CashFlow.Communication.Requests;

namespace CoommonTestsUtilities.Requests;
public class RequestREgisterUserJsonBuilder
{
    public static RequestRegisterJson Build()
    {
        return new Faker<RequestRegisterJson>()
            .RuleFor(user => user.Name, faker => faker.Person.FullName)
            .RuleFor(user => user.Email, (faker, user) => faker.Internet.Email(user.Name))
            .RuleFor(user => user.Password, faker => faker.Internet.Password(prefix: "!Aa1")); 
    }
}
