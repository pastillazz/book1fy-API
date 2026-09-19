using Application.Common.Exceptions;
using Domain.Repositories;
using Microsoft.EntityFrameworkCore;


namespace Infrastructure.Persistence;

internal sealed class UnitOfWork(AppWriteDbContext writeDbContext) : IUnitOfWork
{
    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            await writeDbContext.SaveChangesAsync(cancellationToken);
        }
        catch (DbUpdateConcurrencyException ex)
        {
            writeDbContext.ChangeTracker.Clear();
            throw new ConcurrencyConflictException(ex);
        }
       
    }

   
}
