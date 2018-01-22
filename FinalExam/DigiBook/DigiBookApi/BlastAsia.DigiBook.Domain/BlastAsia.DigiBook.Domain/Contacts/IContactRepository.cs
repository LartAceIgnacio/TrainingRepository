using BlastAsia.DigiBook.Domain.Models.Contacts;
using BlastAsia.DigiBook.Domain.Models.Paginations;

namespace BlastAsia.DigiBook.Domain.Contacts.Interfaces
{
    public interface IContactRepository
        : IRepository<Contact>
    {
        Pagination<Contact> Retrieve(int pageNumber, int recordNumber, string query);
    }
}
