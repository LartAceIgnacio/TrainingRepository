using System;
using System.Collections.Generic;
using System.Text;

namespace BlastAsia.DigiBook.Domain
{

    public interface IRepository<TEntitity>
        where TEntitity : class
    {
        TEntitity Create(TEntitity entitity);
        TEntitity Retrieve(Guid id);
        IEnumerable<TEntitity> Retrieve();
        TEntitity Update(Guid id, TEntitity entity);
        void Delete(Guid id);
    }


}
