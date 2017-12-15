using BlastAsia.DigiBook.Domain.Contacts;
using System;
using System.Collections.Generic;
using System.Text;
using BlastAsia.DigiBook.Domain.Models.Contacts;
using BlastAsia.DigiBook.Insfrastracture.Persistence;

namespace BlastAsia.DigiBook.Infrastracture.Persistence.Repositories
{
    public class ContactRepository
        : RepositoryBase<Contact>, IContactRepository
    {
        public ContactRepository(IDigiBookDbContext context)
            :base(context)
        {

        }
    }
}
