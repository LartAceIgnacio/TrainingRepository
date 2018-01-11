using System;
using System.Collections.Generic;
using System.Text;
using BlastAsia.DigiBook.Domain.Models;

namespace BlastAsia.DigiBook.Domain.Contacts
{
    public interface IContactRepository
        : IRepository<Contact>
    {
        Pagination<Contact> Retrieve(int pageNo, int numRec, string filterValue);
    }
}
