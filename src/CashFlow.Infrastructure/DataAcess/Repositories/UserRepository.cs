﻿using CashFlow.Domain.Entities;
using CashFlow.Domain.Repositories.User;
using Microsoft.EntityFrameworkCore;

namespace CashFlow.Infrastructure.DataAcess.Repositories;
internal class UserRepository : IUserReadOnlyRepository, IUserWriteOnlyRepository
{
    private readonly CashFlowDbContext _dbContext;

    public UserRepository(CashFlowDbContext dbContext) => _dbContext = dbContext;

    public async Task Add(User user)
    {
        await _dbContext.users.AddAsync(user);
    }

    public  async Task<bool> ExisrActiveUserWithEmail(string email)
    {
        return await _dbContext.users.AnyAsync(user => user.Email.Equals(email));
    }

    public async Task<User?> GetUserByEmail(string email)
    {
        return await _dbContext.users.AsNoTracking().FirstOrDefaultAsync(x => x.Email.Equals(email));
    }
}
