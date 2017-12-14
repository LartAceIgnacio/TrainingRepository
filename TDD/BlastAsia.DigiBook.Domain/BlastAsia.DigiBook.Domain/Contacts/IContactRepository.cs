using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BlastAsia.DigiBook.Domain.Models.Contacts;

namespace BlastAsia.DigiBook.Domain.Contacts
{
    public interface IContactRepository
    {
        Contact Create(Contact contact);

        Contact Retrieve(Guid id);

        Contact Update(Guid id, Contact contact);
    }
}
