using Bogus;
using CashFlow.Communication.Requests;

namespace CoommonTestsUtilities.Requests;
public class ReuquestUpdateUserJsonBuilder
{
    public static RequestUpdateUserJson Build() 
    {
        return new Faker<RequestUpdateUserJson>()
            .RuleFor(user => user.Name, faker => faker.Person.FirstName)
            .RuleFor(user => user.Email, (faker, user) => faker.Internet.Email(user.Name));
    }
}
