namespace CashFlow.Domain.Security.Cryptography;
public interface IPasswordEmcripter
{
    string Encrypt(string password);
}
