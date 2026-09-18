
using Domain.Repositories;


namespace Infrastructure.Persistence;

internal sealed class UnitOfWork(AppWriteDbContext writeDbContext) : IUnitOfWork
{
    public Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return writeDbContext.SaveChangesAsync(cancellationToken);
    }

   
}
