using BlastAsia.DigiBook.Domain.Models.Contacts;
using System;
using System.Collections.Generic;
using System.Text;

namespace BlastAsia.DigiBook.Domain.Contacts
{
    public interface IContactService
    {
        Contact Save(Contact contact);
    }
}
