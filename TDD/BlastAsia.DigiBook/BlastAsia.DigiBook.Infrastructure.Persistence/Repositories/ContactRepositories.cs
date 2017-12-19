using BlastAsia.DigiBook.Domain.Contacts;
using System;
using System.Collections.Generic;
using System.Text;
using BlastAsia.DigiBook.Domain.Models.Contacts;

namespace BlastAsia.DigiBook.Infrastructure.Persistence.Repositories
{
    public class ContactRepositories
        : RepositoryBase<Contact>, IContactRepository
    {
        public ContactRepositories(IDigiBookDbContext context): base(context)
        { 

        }
    
    }
}
