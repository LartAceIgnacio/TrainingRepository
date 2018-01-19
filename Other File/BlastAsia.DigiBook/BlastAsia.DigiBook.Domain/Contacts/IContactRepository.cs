using System;

using BlastAsia.DigiBook.Domain.Models.Contacts;
using BlastAsia.DigiBook.Domain.Models.Records;

namespace BlastAsia.DigiBook.Domain.Contacts
{
    public interface IContactRepository
        :IRepository<Contact>
    {
        Record<Contact> Fetch(int pageNo, int numRec, string filterValue);
    }
}
