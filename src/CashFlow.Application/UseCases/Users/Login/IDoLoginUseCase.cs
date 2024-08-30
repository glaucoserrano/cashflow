using CashFlow.Communication.Requests;
using CashFlow.Communication.Responses;

namespace CashFlow.Application.UseCases.Users.Login;
public interface IDoLoginUseCase
{
    Task<ResponseRegisteredJson> Execute(RequestLoginJson request);
}
