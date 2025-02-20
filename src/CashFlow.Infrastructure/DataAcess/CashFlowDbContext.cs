using CashFlow.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CashFlow.Infrastructure.DataAcess;
public class CashFlowDbContext : DbContext
{
    public CashFlowDbContext(DbContextOptions options): base(options) { }
    public DbSet<Expense> expenses { get; set; }
    public DbSet<User> users { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Tag>().ToTable("tags");
    }

}
