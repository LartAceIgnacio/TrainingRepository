using System;
using System.Collections.Generic;
using System.Text;

namespace BlastAsia.Digibook.Domain
{
    public interface IRepository<TEntity>
        where TEntity:class
    {
        TEntity Create(TEntity entity);

        TEntity Retrieve(Guid id);
        TEntity Update(Guid id, TEntity entity);
        void Delete(Guid id);
        IEnumerable<TEntity> Retrieve();
    }
}
