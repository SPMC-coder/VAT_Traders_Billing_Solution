using Microsoft.EntityFrameworkCore;
using VatTraders.Application.Interfaces;
using VatTraders.Infrastructure.Data;

namespace VatTraders.Infrastructure.Repositories;

public class Repository<T> : IRepository<T> where T : class
{
    private readonly VatTradersDbContext _dbContext;
    private readonly DbSet<T> _dbSet;

    public Repository(VatTradersDbContext dbContext)
    {
        _dbContext = dbContext;
        _dbSet = _dbContext.Set<T>();
    }

    public async Task<T?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default) =>
        await _dbSet.FindAsync([id], cancellationToken);

    public async Task<IReadOnlyCollection<T>> ListAsync(CancellationToken cancellationToken = default) =>
        await _dbSet.AsNoTracking().ToListAsync(cancellationToken);

    public async Task AddAsync(T entity, CancellationToken cancellationToken = default) =>
        await _dbSet.AddAsync(entity, cancellationToken);

    public void Update(T entity) => _dbSet.Update(entity);

    public void Remove(T entity) => _dbSet.Remove(entity);
}
