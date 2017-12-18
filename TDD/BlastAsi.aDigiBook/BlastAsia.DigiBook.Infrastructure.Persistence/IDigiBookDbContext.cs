using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlastAsia.DigiBook.Infrastructure.Persistence
{
    public interface IDigiBookDbContext
    {
        DbSet<TEntity> Set<TEntity>() where TEntity : class;

        EntityEntry<TEntity> Update<TEntity>(TEntity entity)
            where TEntity : class;

        int SaveChanges();
    }
}
