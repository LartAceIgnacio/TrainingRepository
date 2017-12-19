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
        private IDigiBookDbContext _dbContext;

        public RepositoryBase(IDigiBookDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IEnumerable<TEntity> Retrieve()
        {
            return _dbContext.Set<TEntity>().ToList();
        }
        public TEntity Create(TEntity entity)
        {
            _dbContext.Set<TEntity>().Add(entity);
            _dbContext.SaveChanges();

            return entity;
        }

        public TEntity Retrieve(Guid entityId)
        {
            return _dbContext.Set<TEntity>().Find(entityId);
        }

        public TEntity Update(Guid entityId, TEntity entity)
        {
            //var contact = this.Retrieve(entityId);
            _dbContext.Set<TEntity>().Update(entity);
            _dbContext.SaveChanges();
            return entity;
        }

        public void Delete(Guid entityId)
        {
            var contact = this.Retrieve(entityId);
            _dbContext.Set<TEntity>().Remove(contact);
            _dbContext.SaveChanges();

        }
    }
}
