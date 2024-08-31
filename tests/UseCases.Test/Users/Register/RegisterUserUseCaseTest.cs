using CashFlow.Application.UseCases.Users.Register;
using CashFlow.Exception;
using CashFlow.Exception.ExceptionsBase;
using CoommonTestsUtilities.Cryptography;
using CoommonTestsUtilities.Mapper;
using CoommonTestsUtilities.Repositories;
using CoommonTestsUtilities.Requests;
using CoommonTestsUtilities.Token;
using FluentAssertions;

namespace UseCases.Test.Users.Register;
public class RegisterUserUseCaseTest
{
    [Fact(DisplayName = nameof(Sucess))]
    public async Task Sucess()
    {
        var request = RequestREgisterUserJsonBuilder.Build();

        var useCase = createUseCase();

        var result = await useCase.Execute(request);

        result.Should().NotBeNull();
        result.Name.Should().Be(request.Name);
        result.Token.Should().NotBeNullOrWhiteSpace();
    }
    [Fact(DisplayName =nameof(Error_Name_Empty))]
    public async Task Error_Name_Empty()
    {
        var request = RequestREgisterUserJsonBuilder.Build();
        request.Name = string.Empty;

        var useCase = createUseCase();

        var act = async () =>  await useCase.Execute(request);

        var result = await act.Should().ThrowAsync<ErrorOnValidateException>();

        result.Where(ex => ex.GetErrors().Count == 1 && ex.GetErrors().Contains((ResourceErrorMessages.NAME_EMPTY)));

    }
    [Fact(DisplayName = nameof(Error_Email_Already_Exist))]
    public async Task Error_Email_Already_Exist() 
    {
        var request = RequestREgisterUserJsonBuilder.Build();

        var useCase = createUseCase(request.Email);

        var act = async () => await useCase.Execute(request);

        var result = await act.Should().ThrowAsync<ErrorOnValidateException>();

        result.Where(ex => ex.GetErrors().Count == 1 && ex.GetErrors().Contains((ResourceErrorMessages.EMAIL_ALREADY_EXISTS)));

    }

    private RegisterUserUseCase createUseCase(string? email = null)
    {
        var mapper = MapperBuilder.Build();
        var unitOfWork = UnitOfWorkBuilder.Build();
        var userWriteOnlyRepository = UserWriteOnlyRepositoryBuilder.Build();
        var readOnlyRepository = new UserReadOnlyRepositoryBuilder();
        var passwordEncript = PasswordEncripterBuilder.Builder();
        var acessToken = JwtTokenGeneratorBuilder.Build();
       
        if(!string.IsNullOrWhiteSpace(email))
            readOnlyRepository.ExisrActiveUserWithEmail(email);

        return new RegisterUserUseCase(
            mapper, 
            passwordEncript,
            readOnlyRepository.Build(),
            userWriteOnlyRepository, 
            unitOfWork, 
            acessToken
            );
    }
}
