using BlastAsia.DigiBook.Domain.Test.Contacts.Contacts;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlastAsia.DigiBook.Domain.Contacts
{
    public interface IContactRepository // MOQ DataBase Injectable
    {
        Contact Create(Contact contact); // Create contact

        Contact Retrieve(Guid contact); // Retrieve contact
        Contact Update(Guid id, Contact contact);
    }
}
