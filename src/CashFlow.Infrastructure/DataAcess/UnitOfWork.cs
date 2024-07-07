using CashFlow.Domain.Repositories;

namespace CashFlow.Infrastructure.DataAcess;
internal class UnitOfWork : IUnitOfWork
{
    private readonly CashFlowDbContext _dbcontext;
    public UnitOfWork(CashFlowDbContext dbcontext)
    {
        _dbcontext = dbcontext;
    }
    public async Task Commit() => await _dbcontext.SaveChangesAsync();
}
