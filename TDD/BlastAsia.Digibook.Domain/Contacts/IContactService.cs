using BlastAsia.Digibook.Domain.Models.Contacts;
using System;

namespace BlastAsia.Digibook.Domain.Contacts
{
    public interface IContactService
    {
        Contact Save(Guid id, Contact contact);
    }
}