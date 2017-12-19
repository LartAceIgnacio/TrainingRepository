using BlastAsia.DigiBook.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BlastAsia.DigiBook.Infrastructure.Persistence
{
    public abstract class BaseRepository<TEntity> : IRepository<TEntity> where TEntity : class
    {
     
        private IDigiBookDbContext _context;

        public BaseRepository(IDigiBookDbContext context)
        {
            this._context = context;
        }

        public void Delete(Guid id)
        {
            var entity = this.Retrieve(id);
            _context.Set<TEntity>().Remove(entity);
            _context.SaveChanges();
        }

        public TEntity Create(TEntity entity)
        {
            _context.Set<TEntity>().Add(entity);
            _context.SaveChanges();
            return entity;
        }

        public TEntity Retrieve(Guid entityId)
        {
            return _context.Set<TEntity>().Find(entityId);

        }

        public TEntity Update(Guid existingEntityId, TEntity entity)
        {
            var found = Retrieve(existingEntityId);
            _context.Update(found);
            _context.SaveChanges();
            return entity;
        }

        public IEnumerable<TEntity> Retrieve()
        {
            return _context.Set<TEntity>().ToList();
        }
    }
}
