using BlastAsia.DigiBook.Domain.Contacts.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using BlastAsia.DigiBook.Domain.Models.Contacts;

namespace BlastAsia.DigiBook.Infrastructure.Persistence
{
    public class ContactRepository : RepositoryBase<Contact>,IContactRepository
    {
        public ContactRepository(IDigiBookDbContext context)
            :base(context)
        {

        }
    }
}
