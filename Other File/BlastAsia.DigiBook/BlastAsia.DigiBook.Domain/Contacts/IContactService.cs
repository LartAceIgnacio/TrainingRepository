using BlastAsia.DigiBook.Domain.Models.Contacts;
using System;

namespace BlastAsia.DigiBook.Domain.Contacts
{
    public interface IContactService
    {
        Contact Save(Guid id, Contact contact);
    }
}