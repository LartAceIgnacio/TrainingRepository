using BlastAsia.DigiBook.Domain.Contacts;
using System;
using System.Collections.Generic;
using System.Text;
using BlastAsia.DigiBook.Domain.Models.Contacts;

namespace BlastAsia.DigiBook.Infrastructure.Persistence.Repositories
{
    public class ContactRepository : IContactRepository
    {
        private DigiBookDbContext _context;

        public ContactRepository(DigiBookDbContext context)
        {
            this._context = context;
        }

        public Contact Create(Contact entity)
        {
            _context.Contacts.Add(entity);
            _context.SaveChanges();
            return entity;
        }

        public Contact Retrieve(Guid contactId)
        {
            throw new NotImplementedException();
        }

        public Contact Update(Guid existingContactId, Contact contact)
        {
            throw new NotImplementedException();
        }
    }
}
