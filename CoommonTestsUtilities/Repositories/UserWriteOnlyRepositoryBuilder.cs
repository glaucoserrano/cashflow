using CashFlow.Domain.Repositories.User;
using Moq;

namespace CoommonTestsUtilities.Repositories;
public class UserWriteOnlyRepositoryBuilder
{
    public static IUserWriteOnlyRepository Build()
    {
        var mock = new Mock<IUserWriteOnlyRepository>();

        return mock.Object;
    }
}
