using System;
using System.Collections.Generic;
using System.Text;
using BlastAsia.DigiBook.Domain.Models.Contacts;
using BlastAsia.DigiBook.Domain.Models.Pagination;

namespace BlastAsia.DigiBook.Domain.Contacts
{
    public interface IContactRepository : IRepository<Contact>

    {
        Pagination<Contact> Retrieve(int pageNumber, int recordNumber, string searchKey);
    }
}
