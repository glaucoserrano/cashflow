namespace CashFlow.Domain.Repositories.User;
public interface IUserReadOnlyRepository
{
    Task<bool> ExisrActiveUserWithEmail(string email);
    Task<Entities.User?> GetUserByEmail(string email);
}
