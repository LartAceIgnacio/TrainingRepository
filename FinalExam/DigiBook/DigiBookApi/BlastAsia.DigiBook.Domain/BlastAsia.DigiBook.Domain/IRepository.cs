using System;
using System.Collections.Generic;
using System.Text;

namespace BlastAsia.DigiBook.Domain
{
    public interface IRepository<TEntity>
        where TEntity: class
    {
        TEntity Create(TEntity entity);
        TEntity Retrieve(Guid entityId);
        IEnumerable<TEntity> Retrieve();
        TEntity Update(Guid entityId, TEntity entity);
        void Delete(Guid entityId);
    }
}
