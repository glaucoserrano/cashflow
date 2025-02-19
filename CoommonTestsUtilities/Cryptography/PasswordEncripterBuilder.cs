using CashFlow.Domain.Security.Cryptography;
using Moq;

namespace CoommonTestsUtilities.Cryptography;
public class PasswordEncripterBuilder
{
    private readonly Mock<IPasswordEmcripter> _mock;

    public PasswordEncripterBuilder()
    {
        _mock = new Mock<IPasswordEmcripter>();

        _mock.Setup(
            config => config.Encrypt(It.IsAny<string>()))
            .Returns("$2a$11$Gv75sK1eVX63oqRyqO3QXea7oK6O4NmmSK2.EmLKOdljyPjwIm5Ci"
            );
    }

    public PasswordEncripterBuilder Verify(string? password = null)
    {
        if (!string.IsNullOrWhiteSpace(password))
            _mock.Setup(
            config => config.Verify(password, It.IsAny<string>()))
            .Returns(true);

        return this;
    }
    public IPasswordEmcripter Build() => _mock.Object;

}
