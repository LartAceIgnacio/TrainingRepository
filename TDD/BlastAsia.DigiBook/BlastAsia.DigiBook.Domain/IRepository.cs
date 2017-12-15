using System;
using System.Collections.Generic;
using System.Text;

namespace BlastAsia.DigiBook.Domain
{
    public interface IRepository<TEntity> where TEntity : class
    {
        TEntity Create(TEntity @object);
        TEntity Retrieve(Guid objectId);
        TEntity Update(Guid existingObjectId, TEntity @object);
    }
}
