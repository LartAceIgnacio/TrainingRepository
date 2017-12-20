using BlastAsia.Digibook.Domain.Contacts;
using System;
using System.Collections.Generic;
using System.Text;
using BlastAsia.Digibook.Domain.Models.Contacts;

namespace BlastAsia.Digibook.Infrastracture.Persistence.Repositories
{
    public class ContactRepository : RepositoryBase<Contact>,IContactRepository
    {
        public ContactRepository(IDigiBookDbContext context):base(context)
        {
        }
    }
}
