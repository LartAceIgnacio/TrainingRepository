using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace BlastAsia.DigiBook.Infrastructure.Persistence
{
    public interface IDigiBookDbContext
    {
        DbSet<TEntity> Set<TEntity>() where TEntity : class;
        int SaveChanges();

        EntityEntry<TEntity> Update<TEntity>(TEntity entity)
            where TEntity : class;
    }
}