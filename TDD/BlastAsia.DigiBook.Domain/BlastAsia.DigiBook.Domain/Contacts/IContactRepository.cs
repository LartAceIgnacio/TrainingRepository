using System;
using System.Collections.Generic;
using System.Text;
using BlastAsia.DigiBook.Domain.Models;
using BlastAsia.DigiBook.Domain.Models.Contacts;

namespace BlastAsia.DigiBook.Domain.Contacts.Interfaces
{
    public interface IContactRepository
    {
        Contact Create(Contact contact);
        Contact Retrieve(Guid contactId);
        Contact Update(Guid contactId, Contact contact);
    }
}
