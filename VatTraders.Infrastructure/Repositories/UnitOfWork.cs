using Microsoft.EntityFrameworkCore.Storage;
using VatTraders.Application.Interfaces;
using VatTraders.Infrastructure.Data;

namespace VatTraders.Infrastructure.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly VatTradersDbContext _dbContext;

    public UnitOfWork(VatTradersDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task ExecuteInTransactionAsync(Func<CancellationToken, Task> operation, CancellationToken cancellationToken = default)
    {
        await using IDbContextTransaction transaction = await _dbContext.Database.BeginTransactionAsync(cancellationToken);
        try
        {
            await operation(cancellationToken);
            await transaction.CommitAsync(cancellationToken);
        }
        catch
        {
            await transaction.RollbackAsync(cancellationToken);
            throw;
        }
    }

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default) =>
        _dbContext.SaveChangesAsync(cancellationToken);
}
