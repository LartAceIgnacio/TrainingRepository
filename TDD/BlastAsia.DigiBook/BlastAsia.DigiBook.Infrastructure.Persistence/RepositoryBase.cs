using BlastAsia.DigiBook.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BlastAsia.DigiBook.Infrastructure.Persistence
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
            context.Set<TEntity>().Add(entity);
            context.SaveChanges();

            return entity;
        }
        public void Delete(Guid id)
        {
            var entity = this.Retrieve(id);
            context.Set<TEntity>().Remove(entity);
            context.SaveChanges();

        }
        public TEntity Retrieve(Guid id)
        {
            return context.Set<TEntity>().Find(id);

        }
        public IEnumerable<TEntity> Retrieve()
        {
            return context.Set<TEntity>().ToList();
        }
        public TEntity Update(Guid entityId, TEntity entity)
        {
            var contact = this.Retrieve(entityId);
            context.Set<TEntity>().Update(contact);
            context.SaveChanges();
            return entity;
        }
    }
}
