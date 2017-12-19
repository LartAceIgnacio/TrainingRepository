using System;
using System.Collections.Generic;
using System.Text;

namespace BlastAsia.DigiBook.Domain
{
    public interface IRepository<TEntity>
        where TEntity: class
    {
        TEntity Create(TEntity entity);
        TEntity Retrieve(Guid contactId);
        TEntity Update(Guid id, TEntity entity);
        IEnumerable<TEntity> Retreive();
        void Delete(Guid id);
    }
}
