﻿using CashFlow.Domain.Entities;
using CashFlow.Domain.Repositories.User;
using Moq;

namespace CoommonTestsUtilities.Repositories;
public class UserReadOnlyRepositoryBuilder
{
    private readonly Mock<IUserReadOnlyRepository> _repository;

    public UserReadOnlyRepositoryBuilder()
    {
        _repository = new Mock<IUserReadOnlyRepository>();
    }
    public void ExisrActiveUserWithEmail(string email)
    {
        _repository.Setup(config => config.ExisrActiveUserWithEmail(email)).ReturnsAsync(true);
    }
    public void GetUserByEmail(User user)
    {
        _repository.Setup(config => config.GetUserByEmail(user.Email)).ReturnsAsync(user);
    }
    public IUserReadOnlyRepository Build() => _repository.Object;
}
