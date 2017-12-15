using BlastAsia.DigiBook.Domain.Contacts;
using System;
using System.Collections.Generic;
using System.Text;
using BlastAsia.DigiBook.Domain.Models.Contacts;

namespace BlastAsia.DigiBook.Infrastructure.Persistence.Repositories
{
   public class ContactRepository
        : IContactRepository
    {
        private readonly DigiBookDbContext context;
        public ContactRepository(DigiBookDbContext context)
        {
            this.context = context;
        }

        public Contact Create(Contact entity)
        {
            context.Contacts.Add(entity);
            context.SaveChanges();

            return entity;
        }

        public Contact Retrieve(Guid id)
        {
            throw new NotImplementedException();
        }

        public Contact Update(Guid id, Contact entity)
        {
            throw new NotImplementedException();
        }
    }
}
