using CashFlow.Communication.Requests;
using CashFlow.Communication.Responses;
using CashFlow.Domain.Repositories.User;
using CashFlow.Domain.Security.Cryptography;
using CashFlow.Domain.Security.Token;
using CashFlow.Exception.ExceptionsBase;

namespace CashFlow.Application.UseCases.Users.Login;
public class DoLoginUseCase : IDoLoginUseCase
{
    private readonly IUserReadOnlyRepository _repository;
    private readonly IPasswordEmcripter _passwordEmcripter;
    private readonly IAccessTokenGenerator _tokenGenerator;

    public DoLoginUseCase(
        IUserReadOnlyRepository repository, 
        IPasswordEmcripter passwordEmcripter, 
        IAccessTokenGenerator tokenGenerator
        )
    {
        _repository = repository;
        _passwordEmcripter = passwordEmcripter;
        _tokenGenerator = tokenGenerator;
    }

    public async Task<ResponseRegisteredJson> Execute(RequestLoginJson request)
    {
       
        var user = await _repository.GetUserByEmail(request.Email);
        if(user is null)
            throw new InvalidLoginException();

        var passwordMatch = _passwordEmcripter.Verify(request.Password, user.Password);

        if(!passwordMatch)
            throw new InvalidLoginException();

        return new ResponseRegisteredJson
        {
            Name = user.Name,
            Token = _tokenGenerator.Generate(user)
        };
    }
}
