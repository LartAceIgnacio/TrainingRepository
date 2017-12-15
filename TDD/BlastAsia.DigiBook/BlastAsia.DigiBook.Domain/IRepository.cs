using System;
using System.Collections.Generic;
using System.Text;

namespace BlastAsia.DigiBook.Domain
{
    public interface IRepository<TEntity>
    {
        TEntity Create(TEntity entity);
        TEntity Retrieve(Guid id);
        TEntity Update(Guid id, TEntity entity);
    }
}
