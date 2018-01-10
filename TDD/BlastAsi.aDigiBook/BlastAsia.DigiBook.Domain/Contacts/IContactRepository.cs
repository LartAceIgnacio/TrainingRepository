using System;
using System.Collections.Generic;
using System.Text;
using BlastAsia.DigiBook.Domain.Models;
using BlastAsia.DigiBook.Domain.Models.Contacts;

namespace BlastAsia.DigiBook.Domain.Contacts
{
    public interface IContactRepository 
        : IRepository <Contact>
    {
        PaginationResult<Contact> Retrieve(int pageNo, int numRec, string filterValue);
    }
}
