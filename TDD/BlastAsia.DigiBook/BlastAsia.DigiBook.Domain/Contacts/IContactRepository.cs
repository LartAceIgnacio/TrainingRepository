
using BlastAsia.DigiBook.Domain.Models.Contacts;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlastAsia.DigiBook.Domain.Contacts
{
    public interface IContactRepository
    {
        Contact Create(Contact contact);
        Contact Retrieve(Guid contactId);
        Contact Update(Guid existingContactId, Contact contact);
    }
}