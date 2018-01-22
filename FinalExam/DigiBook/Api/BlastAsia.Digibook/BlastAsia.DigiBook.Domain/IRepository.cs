using System;
using System.Collections.Generic;
using System.Text;

namespace BlastAsia.DigiBook.Domain
{
    public interface IRepository<TEntity>
        where TEntity : class
    {
        TEntity Create(TEntity entitity);
        TEntity Retrieve(Guid id);
        IEnumerable<TEntity> Retrieve();
        //IEnumerable<TEntity> Retrieve(string word);
        //IEnumerable<TEntity> Retrieve(int pageNumber, int recordNumber);
        TEntity Update(Guid id, TEntity entity);
        void Delete(Guid id);
    }

}
