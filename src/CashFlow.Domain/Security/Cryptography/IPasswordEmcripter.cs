namespace CashFlow.Domain.Security.Cryptography;
public interface IPasswordEmcripter
{
    string Encrypt(string password);
    bool Verify(string password, string passwordHash);
}
