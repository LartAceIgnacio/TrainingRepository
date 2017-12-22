﻿using System;
using System.Collections.Generic;

namespace BlastAsia.DigiBook.Domain
{
    public interface IRepository<TEntity>
    {
        TEntity Create(TEntity entity);
        TEntity Retrieve(Guid id);
        IEnumerable<TEntity> Retrieve();
        TEntity Update(Guid id, TEntity entity);
        void Delete(Guid id);
    }
}