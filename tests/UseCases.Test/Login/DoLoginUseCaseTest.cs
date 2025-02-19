﻿using CashFlow.Application.UseCases.Users.Login;
using CashFlow.Domain.Entities;
using CashFlow.Exception;
using CashFlow.Exception.ExceptionsBase;
using CoommonTestsUtilities.Cryptography;
using CoommonTestsUtilities.Entities;
using CoommonTestsUtilities.Repositories;
using CoommonTestsUtilities.Requests;
using CoommonTestsUtilities.Token;
using FluentAssertions;

namespace UseCases.Test.Login;
public class DoLoginUseCaseTest
{
    [Fact(DisplayName = nameof(Sucess))]
    public async Task Sucess() 
    {
        var request = RequestLoginJsonBuilder.Build();
        var user = UserBuilder.Build();
        request.Email = user.Email;

        var useCase = CreateUseCase(user,request.Password);

        var result = await useCase.Execute(request);

        result.Should().NotBeNull();
        result.Name.Should().Be(user.Name);
        result.Token.Should().NotBeNullOrWhiteSpace();
    }

    [Fact(DisplayName = nameof(Error_User_NotFound))]
    public async Task Error_User_NotFound() 
    {
        var request = RequestLoginJsonBuilder.Build();
        var user = UserBuilder.Build();
        
        var useCase = CreateUseCase(user, request.Password);

        var act = async () => await useCase.Execute(request);

        var result = await act.Should().ThrowAsync<InvalidLoginException>();

        result.Where(ex => ex.GetErrors().Count == 1 && ex.GetErrors().Contains(ResourceErrorMessages.EMAIL_OR_PASSWORD_INVALID));
    }

    [Fact(DisplayName = nameof(Error_Password_Not_Match))]
    public async Task Error_Password_Not_Match() 
    {
        var request = RequestLoginJsonBuilder.Build();
        var user = UserBuilder.Build();
        request.Email = user.Email;

        var useCase = CreateUseCase(user);

        var act = async () => await useCase.Execute(request);

        var result = await act.Should().ThrowAsync<InvalidLoginException>();

        result.Where(ex => ex.GetErrors().Count == 1 && ex.GetErrors().Contains(ResourceErrorMessages.EMAIL_OR_PASSWORD_INVALID));
    }


    private DoLoginUseCase CreateUseCase(User user, string? password = null)
    {
        var passwordEncripter = new PasswordEncripterBuilder().Verify(password).Build();
        var tokenGeneration = JwtTokenGeneratorBuilder.Build();
        var readOnlyRepository = new UserReadOnlyRepositoryBuilder();

        readOnlyRepository.GetUserByEmail(user);


        return new DoLoginUseCase(readOnlyRepository.Build(), passwordEncripter, tokenGeneration);
    }
}
