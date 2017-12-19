using BlastAsia.DigiBook.Domain;
using BlastAsia.DigiBook.Insfrastracture.Persistence;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BlastAsia.DigiBook.Infrastracture.Persistence
{
    public abstract class RepositoryBase <TEntity>
        : IRepository<TEntity> where TEntity : class
    {
        private readonly IDigiBookDbContext _context;
        public RepositoryBase(IDigiBookDbContext context)
        {
            _context = context;
        }

        public TEntity Create(TEntity entity)
        {
            _context.Set<TEntity>().Add(entity);
            _context.SaveChanges();
            return entity;
        }

        public void Delete(Guid id)
        {
            var contact = this.Retrieve(id);
            _context.Set<TEntity>().Remove(contact);
            _context.SaveChanges();
        }

        public TEntity Retrieve(Guid id)
        {
            return _context.Set<TEntity>().Find(id);
        }

        public IEnumerable<TEntity> Retrieve()
        {
            return _context.Set<TEntity>().ToList();
        }

        public TEntity Update(Guid id, TEntity entity)
        {
            _context.Update(entity);
            _context.SaveChanges();
            return entity;
        }
    }
}
