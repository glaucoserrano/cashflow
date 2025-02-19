using Bogus;
using CashFlow.Domain.Entities;
using CashFlow.Domain.Enum;
using CoommonTestsUtilities.Cryptography;

namespace CoommonTestsUtilities.Entities;
public class UserBuilder
{
    public static User Build(string role = Roles.TEAM_MEMBER)
    {
        var passwordEncripter = new PasswordEncripterBuilder().Build();

        var user = new Faker<User>()
            .RuleFor(user => user.Id, _ => 1)
            .RuleFor(user => user.Name, faker => faker.Person.FullName)
            .RuleFor(user => user.Email, (faker, user) => faker.Internet.Email(user.Name))
            .RuleFor(user => user.Password, (_, user) => passwordEncripter.Encrypt(user.Password))
            .RuleFor(user => user.Role,_ => role)
            .RuleFor(user => user.UserIdentifier, _ => Guid.NewGuid());
            
        return user;
    }
}
