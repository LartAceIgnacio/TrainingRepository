using BlastAsia.DigiBook.Domain;
using BlastAsia.DigiBook.Insfrastracture.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BlastAsia.DigiBook.Infrastracture.Persistence
{
    public abstract class RepositoryBase<TEntity>
        : IRepository<TEntity> where TEntity : class
    {
        private readonly IDigiBookDbContext context;
        public RepositoryBase(IDigiBookDbContext context)
        {
            this.context = context;
        }

        public TEntity Create(TEntity entity)
        {
            this.context.Set<TEntity>().Add(entity);
            this.context.SaveChanges();
            return entity;
        }

        public void Delete(Guid id)
        {
            var contact = this.Retrieve(id);
            this.context.Set<TEntity>().Remove(contact);
            this.context.SaveChanges();
        }

        public TEntity Retrieve(Guid id)
        {
            return this.context.Set<TEntity>().Find(id);
        }

        public IEnumerable<TEntity> Retrieve()
        {
            return this.context.Set<TEntity>().ToList();
        }

        public TEntity Update(Guid id, TEntity entity)
        {
            this.context.Update(entity);
            this.context.SaveChanges();
            return entity;
        }
    }
}
