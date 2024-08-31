using CashFlow.Domain.Security.Cryptography;
using Moq;

namespace CoommonTestsUtilities.Cryptography;
public class PasswordEncripterBuilder
{
    public static IPasswordEmcripter Builder()
    {
        var mock = new Mock<IPasswordEmcripter>();

        mock.Setup(
            config => config.Encrypt(It.IsAny<string>()))
            .Returns("$2a$11$Gv75sK1eVX63oqRyqO3QXea7oK6O4NmmSK2.EmLKOdljyPjwIm5Ci"
            );

        return mock.Object;
    }
}
