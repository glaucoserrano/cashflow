﻿using CashFlow.Application.UseCases.Users.ChangePassword;
using CashFlow.Domain.Entities;
using CashFlow.Exception;
using CashFlow.Exception.ExceptionsBase;
using CoommonTestsUtilities;
using CoommonTestsUtilities.Cryptography;
using CoommonTestsUtilities.Entities;
using CoommonTestsUtilities.Repositories;
using CoommonTestsUtilities.Requests;
using FluentAssertions;

namespace UseCases.Test.Users.ChangePassword;
public class ChangePasswordUseCaseTest
{
    [Fact(DisplayName = nameof(Sucess))]
    public async Task Sucess()
    {
        var user = UserBuilder.Build();

        var request = RequestChangePasswordJsonBuilder.Build();

        var useCase = CreateUseCase(user, request.Password);

        var act = async () => await useCase.Execute(request);

        await act.Should().NotThrowAsync();
    }
    [Fact(DisplayName = nameof(Error_NewPassword_Empty))]
    public async Task Error_NewPassword_Empty()
    {
        var user = UserBuilder.Build();
        var request = RequestChangePasswordJsonBuilder.Build();
        request.NewPassword = string.Empty;

        var useCase = CreateUseCase(user, request.Password);
        var act = async () => { await useCase.Execute(request); };

        var result = await act.Should().ThrowAsync<ErrorOnValidateException>();

        result.Where(e => e.GetErrors().Count == 1 &&
            e.GetErrors().Contains(ResourceErrorMessages.INVALID_PASSWORD));
    }

    [Fact(DisplayName = nameof(Error_CurrentPassword_Different))]
    public async Task Error_CurrentPassword_Different()
    {
        var user = UserBuilder.Build();
        var request = RequestChangePasswordJsonBuilder.Build();

        var useCase = CreateUseCase(user);

        var act = async () => { await useCase.Execute(request); };

        var result = await act.Should().ThrowAsync<ErrorOnValidateException>();
        result.Where(e => e.GetErrors().Count == 1 &&
            e.GetErrors().Contains(ResourceErrorMessages.PASSWORD_DIFFERENT_CURRENT_PASSWORD));
    }
    private static ChangePasswordUseCase CreateUseCase(User user, string? password = null)
    {
        var unitOfWork = UnitOfWorkBuilder.Build();
        var userUpdateRepository = UserUpdateOnlyRepositoryBuilder.Build(user);
        var loggedUser = LoggedUserBuilder.Build(user);
        var passwordEncripter = new PasswordEncripterBuilder().Verify(password).Build();

        return new ChangePasswordUseCase(loggedUser, userUpdateRepository, unitOfWork, passwordEncripter);
    }
}
